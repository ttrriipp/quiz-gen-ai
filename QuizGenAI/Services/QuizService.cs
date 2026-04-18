using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public List<QuizListItemDto> GetQuizSummaries(string searchTerm, QuizStatus? status)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var query = context.Quizzes
                    .Include(x => x.Subject)
                    .Include(x => x.Questions)
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
                        UpdatedAt = x.UpdatedAt
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

                if (quiz == null)
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
                                    Text = c.Text,
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
                        .SingleOrDefault(x => x.Id == quizEditor.Id);

                    if (quiz == null)
                    {
                        throw new InvalidOperationException("Quiz not found.");
                    }

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
                            Text = choiceEditor.Text.Trim(),
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

                return quiz.Id;
            }
        }

        public void DeleteQuiz(int quizId, int userId)
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

                foreach (var question in quiz.Questions.ToList())
                {
                    context.Choices.RemoveRange(question.Choices.ToList());
                }

                context.Questions.RemoveRange(quiz.Questions.ToList());
                context.Quizzes.Remove(quiz);
                context.SaveChanges();
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
    }
}
