using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using QuizGenAI.Data;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Models;

namespace QuizGenAI.Services
{
    public class QuizService
    {
        public List<SubjectOptionDto> GetSubjects()
        {
            using (var context = new QuizGenAIDbContext())
            {
                return context.Subjects
                    .OrderBy(x => x.Name)
                    .Select(x => new SubjectOptionDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();
            }
        }

        public List<StudentQuizListItemDto> GetStudentQuizList(int studentId)
        {
            if (studentId <= 0)
            {
                return new List<StudentQuizListItemDto>();
            }

            using (var context = new QuizGenAIDbContext())
            {
                var now = DateTime.Now;
                var quizzes = context.Quizzes
                    .Include(x => x.Subject)
                    .Include(x => x.Questions)
                    .Include(x => x.StudentAttempts)
                    .Where(x => !x.IsDeleted && x.Status == QuizStatus.Published)
                    .OrderBy(x => x.AvailableFrom ?? x.UpdatedAt)
                    .ThenBy(x => x.Title)
                    .ToList();

                return quizzes.Select(x =>
                    {
                        var attempts = x.StudentAttempts
                            .Where(a => a.StudentId == studentId)
                            .OrderByDescending(a => a.StartedAt)
                            .ToList();
                        var hasCompletedAttempt = attempts.Any(a => a.SubmittedAt.HasValue);
                        var bestSubmittedScore = attempts
                            .Where(a => a.SubmittedAt.HasValue && a.ScorePercentage.HasValue)
                            .Select(a => a.ScorePercentage)
                            .DefaultIfEmpty()
                            .Max();

                        var withinAvailabilityWindow =
                            (!x.AvailableFrom.HasValue || x.AvailableFrom.Value <= now) &&
                            (!x.AvailableUntil.HasValue || x.AvailableUntil.Value >= now);
                        var canStart = withinAvailabilityWindow && !hasCompletedAttempt;
                        var availabilityLabel = hasCompletedAttempt
                            ? (bestSubmittedScore.HasValue
                                ? string.Format("Completed - Final score: {0:0.#}%", bestSubmittedScore.Value)
                                : "Completed - Final score submitted")
                            : BuildAvailabilityLabel(x.AvailableFrom, x.AvailableUntil, now);

                        return new StudentQuizListItemDto
                        {
                            Id = x.Id,
                            Title = x.Title,
                            SubjectName = x.Subject != null ? x.Subject.Name : "Unknown Subject",
                            Topic = x.Topic,
                            Difficulty = x.Difficulty,
                            DurationMinutes = x.DurationMinutes,
                            QuestionCount = x.Questions.Count,
                            AvailableFrom = x.AvailableFrom,
                            AvailableUntil = x.AvailableUntil,
                            CanStart = canStart,
                            AvailabilityLabel = availabilityLabel,
                            AttemptCount = attempts.Count,
                            HasCompletedAttempt = hasCompletedAttempt,
                            BestScore = bestSubmittedScore
                        };
                    })
                    .ToList();
            }
        }

        public List<QuizListItemDto> GetQuizSummaries(string searchTerm, QuizStatus? status)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var query = context.Quizzes
                    .Include(x => x.Subject)
                    .Include(x => x.Questions)
                    .Include(x => x.StudentAttempts)
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var search = searchTerm.Trim().ToLower();
                    query = query.Where(x =>
                        x.Title.ToLower().Contains(search) ||
                        x.Topic.ToLower().Contains(search) ||
                        x.Subject.Name.ToLower().Contains(search));
                }

                if (status.HasValue)
                {
                    query = query.Where(x => x.Status == status.Value);
                }

                return query
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToList()
                    .Select(x => new QuizListItemDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        SubjectName = x.Subject != null ? x.Subject.Name : "Unknown Subject",
                        Topic = x.Topic,
                        Difficulty = x.Difficulty,
                        Status = x.Status,
                        DurationMinutes = x.DurationMinutes,
                        QuestionCount = x.Questions.Count,
                        UpdatedAt = x.UpdatedAt,
                        CanPublish = x.Questions.Count > 0,
                        IsLockedForEditing = x.Status == QuizStatus.Published && x.StudentAttempts.Any(a => a.SubmittedAt.HasValue),
                        AvailableFrom = x.AvailableFrom,
                        AvailableUntil = x.AvailableUntil
                    })
                    .ToList();
            }
        }

        public QuizEditorDto GetQuizEditor(int quizId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var quiz = context.Quizzes
                    .Include(x => x.Questions.Select(q => q.Choices))
                    .SingleOrDefault(x => x.Id == quizId);

                if (quiz == null || quiz.IsDeleted)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                return new QuizEditorDto
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    SubjectId = quiz.SubjectId,
                    Topic = quiz.Topic,
                    Difficulty = quiz.Difficulty,
                    DurationMinutes = quiz.DurationMinutes,
                    Status = quiz.Status,
                    IsAiGenerated = quiz.AiGenerated,
                    AvailableFrom = quiz.AvailableFrom,
                    AvailableUntil = quiz.AvailableUntil,
                    Questions = quiz.Questions
                        .OrderBy(x => x.OrderIndex)
                        .Select(x => new QuizQuestionEditorDto
                        {
                            Text = x.Text,
                            Explanation = x.Explanation,
                            Choices = x.Choices
                                .OrderBy(c => c.OrderIndex)
                                .Select(c => new QuizChoiceEditorDto
                                {
                                    Text = StripChoicePrefix(c.Text),
                                    IsCorrect = c.IsCorrect
                                })
                                .ToList()
                        })
                        .ToList()
                };
            }
        }

        public int SaveDraft(QuizEditorDto quizEditor, int userId)
        {
            if (quizEditor == null)
            {
                throw new InvalidOperationException("Quiz data is required.");
            }

            ValidateQuiz(quizEditor);

            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var now = DateTime.UtcNow;
                Quiz quiz;
                var isNewQuiz = quizEditor.Id <= 0;

                if (quizEditor.Id > 0)
                {
                    quiz = context.Quizzes
                        .Include(x => x.Questions.Select(q => q.Choices))
                        .Include(x => x.StudentAttempts)
                        .SingleOrDefault(x => x.Id == quizEditor.Id);

                    if (quiz == null)
                    {
                        throw new InvalidOperationException("Quiz not found.");
                    }

                    if (quiz.IsDeleted)
                    {
                        throw new InvalidOperationException("This quiz is no longer available.");
                    }

                    EnsureQuizEditable(quiz);

                    foreach (var question in quiz.Questions.ToList())
                    {
                        context.Choices.RemoveRange(question.Choices.ToList());
                    }

                    context.Questions.RemoveRange(quiz.Questions.ToList());
                }
                else
                {
                    quiz = new Quiz
                    {
                        CreatedAt = now,
                        CreatedBy = userId,
                        AiGenerated = false
                    };
                    context.Quizzes.Add(quiz);
                }

                quiz.Title = quizEditor.Title.Trim();
                quiz.SubjectId = quizEditor.SubjectId;
                quiz.Topic = quizEditor.Topic.Trim();
                quiz.Difficulty = quizEditor.Difficulty;
                quiz.DurationMinutes = quizEditor.DurationMinutes;
                quiz.Status = QuizStatus.Draft;
                quiz.UpdatedAt = now;
                quiz.AiGenerated = quizEditor.IsAiGenerated;
                quiz.AvailableFrom = quizEditor.AvailableFrom;
                quiz.AvailableUntil = quizEditor.AvailableUntil;

                var questionOrder = 1;
                foreach (var questionEditor in quizEditor.Questions)
                {
                    var question = new Question
                    {
                        Text = questionEditor.Text.Trim(),
                        Explanation = string.IsNullOrWhiteSpace(questionEditor.Explanation) ? null : questionEditor.Explanation.Trim(),
                        QuestionType = QuestionType.MultipleChoice,
                        OrderIndex = questionOrder,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    var choiceOrder = 1;
                    foreach (var choiceEditor in questionEditor.Choices.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
                    {
                        question.Choices.Add(new Choice
                        {
                            Text = StripChoicePrefix(choiceEditor.Text),
                            IsCorrect = choiceEditor.IsCorrect,
                            OrderIndex = choiceOrder
                        });

                        choiceOrder++;
                    }

                    quiz.Questions.Add(question);
                    questionOrder++;
                }

                context.SaveChanges();

                if (isNewQuiz &&
                    quizEditor.IsAiGenerated &&
                    !string.IsNullOrWhiteSpace(quizEditor.AiGenerationPrompt))
                {
                    context.AiGenerations.Add(new AiGeneration
                    {
                        QuizId = quiz.Id,
                        Prompt = quizEditor.AiGenerationPrompt,
                        RawResponseJson = quizEditor.AiGenerationRawResponseJson,
                        Provider = quizEditor.AiGenerationProvider,
                        ModelName = quizEditor.AiGenerationModelName,
                        CreatedAt = now
                    });
                    context.SaveChanges();
                }

                LoggingService.Information(
                    "Quiz draft saved. QuizId={QuizId} UserId={UserId} IsNew={IsNewQuiz} AiGenerated={AiGenerated}",
                    quiz.Id,
                    userId,
                    isNewQuiz,
                    quiz.AiGenerated);
                return quiz.Id;
            }
        }

        public bool IsQuizLockedForEditing(int quizId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var quiz = context.Quizzes
                    .Include(x => x.StudentAttempts)
                    .SingleOrDefault(x => x.Id == quizId);

                if (quiz == null || quiz.IsDeleted)
                {
                    return false;
                }

                return quiz.Status == QuizStatus.Published && quiz.StudentAttempts.Any(x => x.SubmittedAt.HasValue);
            }
        }

        public void DeleteQuiz(int quizId, int userId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var quiz = context.Quizzes
                    .SingleOrDefault(x => x.Id == quizId);

                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                if (quiz.IsDeleted)
                {
                    return;
                }

                var now = DateTime.UtcNow;
                quiz.IsDeleted = true;
                quiz.UpdatedAt = now;
                context.SaveChanges();

                LoggingService.Information("Quiz soft-deleted. QuizId={QuizId} UserId={UserId} Title={Title}", quizId, userId, quiz.Title);
            }
        }

        public void PublishQuiz(int quizId, int userId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var quiz = context.Quizzes
                    .Include(x => x.Questions.Select(q => q.Choices))
                    .SingleOrDefault(x => x.Id == quizId);

                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                if (quiz.IsDeleted)
                {
                    throw new InvalidOperationException("This quiz is no longer available.");
                }

                if (quiz.Status == QuizStatus.Archived)
                {
                    throw new InvalidOperationException("Archived quizzes cannot be published directly.");
                }

                ValidatePublishReadiness(quiz);

                quiz.Status = QuizStatus.Published;
                quiz.UpdatedAt = DateTime.UtcNow;
                context.SaveChanges();

                LoggingService.Information("Quiz published. QuizId={QuizId} UserId={UserId} Title={Title}", quizId, userId, quiz.Title);
            }
        }

        public void UnpublishQuiz(int quizId, int userId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var quiz = context.Quizzes.SingleOrDefault(x => x.Id == quizId);
                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                if (quiz.IsDeleted)
                {
                    throw new InvalidOperationException("This quiz is no longer available.");
                }

                if (quiz.Status != QuizStatus.Published)
                {
                    throw new InvalidOperationException("Only published quizzes can be moved back to draft.");
                }

                quiz.Status = QuizStatus.Draft;
                quiz.UpdatedAt = DateTime.UtcNow;
                context.SaveChanges();

                LoggingService.Information("Quiz moved back to draft. QuizId={QuizId} UserId={UserId} Title={Title}", quizId, userId, quiz.Title);
            }
        }

        public void ArchiveQuiz(int quizId, int userId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var quiz = context.Quizzes.SingleOrDefault(x => x.Id == quizId);
                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                if (quiz.IsDeleted)
                {
                    throw new InvalidOperationException("This quiz is no longer available.");
                }

                if (quiz.Status == QuizStatus.Archived)
                {
                    throw new InvalidOperationException("Quiz is already archived.");
                }

                quiz.Status = QuizStatus.Archived;
                quiz.UpdatedAt = DateTime.UtcNow;
                context.SaveChanges();

                LoggingService.Information("Quiz archived. QuizId={QuizId} UserId={UserId} Title={Title}", quizId, userId, quiz.Title);
            }
        }

        public void UnarchiveQuiz(int quizId, int userId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureTeacherOrAdmin(context, userId);

                var quiz = context.Quizzes.SingleOrDefault(x => x.Id == quizId);
                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                if (quiz.IsDeleted)
                {
                    throw new InvalidOperationException("This quiz is no longer available.");
                }

                if (quiz.Status != QuizStatus.Archived)
                {
                    throw new InvalidOperationException("Only archived quizzes can be restored.");
                }

                quiz.Status = QuizStatus.Draft;
                quiz.UpdatedAt = DateTime.UtcNow;
                context.SaveChanges();

                LoggingService.Information("Quiz unarchived to draft. QuizId={QuizId} UserId={UserId} Title={Title}", quizId, userId, quiz.Title);
            }
        }

        private static void EnsureTeacherOrAdmin(QuizGenAIDbContext context, int userId)
        {
            var allowed = context.UserRoles.Any(x =>
                x.UserId == userId &&
                (x.Role == UserRole.Admin || x.Role == UserRole.Teacher));

            if (!allowed)
            {
                throw new InvalidOperationException("Only teachers and admins can manage quizzes.");
            }
        }

        private static void ValidateQuiz(QuizEditorDto quizEditor)
        {
            if (string.IsNullOrWhiteSpace(quizEditor.Title))
            {
                throw new InvalidOperationException("Quiz title is required.");
            }

            if (quizEditor.SubjectId <= 0)
            {
                throw new InvalidOperationException("Subject is required.");
            }

            if (string.IsNullOrWhiteSpace(quizEditor.Topic))
            {
                throw new InvalidOperationException("Topic is required.");
            }

            if (quizEditor.DurationMinutes <= 0)
            {
                throw new InvalidOperationException("Duration must be greater than zero.");
            }

            if (!Enum.IsDefined(typeof(QuizDifficulty), quizEditor.Difficulty))
            {
                throw new InvalidOperationException("Difficulty is required.");
            }

            if (quizEditor.AvailableFrom.HasValue &&
                quizEditor.AvailableUntil.HasValue &&
                quizEditor.AvailableUntil.Value <= quizEditor.AvailableFrom.Value)
            {
                throw new InvalidOperationException("Available until must be later than available from.");
            }

            if (quizEditor.Questions == null || quizEditor.Questions.Count == 0)
            {
                throw new InvalidOperationException("A quiz draft must contain at least one question.");
            }

            foreach (var question in quizEditor.Questions)
            {
                if (string.IsNullOrWhiteSpace(question.Text))
                {
                    throw new InvalidOperationException("Each question must have text.");
                }

                var validChoices = question.Choices == null
                    ? new List<QuizChoiceEditorDto>()
                    : question.Choices.Where(x => !string.IsNullOrWhiteSpace(x.Text)).ToList();

                if (validChoices.Count < 2)
                {
                    throw new InvalidOperationException("Each question must have at least two choices.");
                }

                if (validChoices.Count(x => x.IsCorrect) != 1)
                {
                    throw new InvalidOperationException("Each question must have exactly one correct choice.");
                }
            }
        }

        private static void ValidatePublishReadiness(Quiz quiz)
        {
            if (quiz.Questions == null || !quiz.Questions.Any())
            {
                throw new InvalidOperationException("A quiz must contain at least one question before publishing.");
            }

            foreach (var question in quiz.Questions)
            {
                if (string.IsNullOrWhiteSpace(question.Text))
                {
                    throw new InvalidOperationException("All published questions must have text.");
                }

                var choices = question.Choices == null
                    ? new List<Choice>()
                    : question.Choices.Where(x => !string.IsNullOrWhiteSpace(x.Text)).ToList();

                if (choices.Count < 2)
                {
                    throw new InvalidOperationException("Each published question must have at least two choices.");
                }

                if (choices.Count(x => x.IsCorrect) != 1)
                {
                    throw new InvalidOperationException("Each published question must have exactly one correct choice.");
                }
            }
        }

        private static void EnsureQuizEditable(Quiz quiz)
        {
            if (quiz.Status == QuizStatus.Published &&
                quiz.StudentAttempts != null &&
                quiz.StudentAttempts.Any(x => x.SubmittedAt.HasValue))
            {
                throw new InvalidOperationException("This posted quiz already has student submissions and is now view-only.");
            }
        }

        private static string BuildAvailabilityLabel(DateTime? availableFrom, DateTime? availableUntil, DateTime now)
        {
            if (availableFrom.HasValue && availableFrom.Value > now)
            {
                return string.Format("Opens {0}", availableFrom.Value.ToString("MMM d, yyyy h:mm tt"));
            }

            if (availableUntil.HasValue && availableUntil.Value < now)
            {
                return string.Format("Closed {0}", availableUntil.Value.ToString("MMM d, yyyy h:mm tt"));
            }

            if (availableUntil.HasValue)
            {
                return string.Format("Available until {0}", availableUntil.Value.ToString("MMM d, yyyy h:mm tt"));
            }

            return "Available now";
        }

        private static string StripChoicePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return Regex.Replace(value.Trim(), @"^(?:choice|option|answer)?\s*[\(\[]?(?:[A-Da-d]|[1-4])[\)\].:-]\s*", string.Empty).Trim();
        }
    }
}
