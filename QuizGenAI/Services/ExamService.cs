using System;
using System.Data.Entity;
using System.Linq;
using QuizGenAI.Data;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Models;

namespace QuizGenAI.Services
{
    public class ExamService
    {
        public int StartAttempt(int studentId, int quizId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                EnsureStudent(context, studentId);

                var quiz = context.Quizzes
                    .Include(x => x.Questions.Select(q => q.Choices))
                    .SingleOrDefault(x => x.Id == quizId);

                if (quiz == null)
                {
                    throw new InvalidOperationException("Quiz not found.");
                }

                ValidateStudentCanAccessQuiz(quiz);

                var existingAttempt = context.StudentAttempts
                    .Include(x => x.Answers)
                    .Where(x => x.StudentId == studentId && x.QuizId == quizId && !x.SubmittedAt.HasValue)
                    .OrderByDescending(x => x.StartedAt)
                    .FirstOrDefault();

                if (existingAttempt != null)
                {
                    LoggingService.Information("Exam attempt resumed. AttemptId={AttemptId} StudentId={StudentId} QuizId={QuizId}", existingAttempt.Id, studentId, quizId);
                    return existingAttempt.Id;
                }

                var now = DateTime.UtcNow;
                var nextAttemptNumber = context.StudentAttempts
                    .Where(x => x.StudentId == studentId && x.QuizId == quizId)
                    .Select(x => (int?)x.AttemptNumber)
                    .DefaultIfEmpty(0)
                    .Max()
                    .GetValueOrDefault() + 1;

                var attempt = new StudentAttempt
                {
                    QuizId = quizId,
                    StudentId = studentId,
                    AttemptNumber = nextAttemptNumber,
                    StartedAt = now,
                    TimeSpentSeconds = 0
                };

                foreach (var question in quiz.Questions.OrderBy(x => x.OrderIndex))
                {
                    attempt.Answers.Add(new StudentAnswer
                    {
                        QuestionId = question.Id,
                        AnsweredAt = now,
                        IsCorrect = false
                    });
                }

                context.StudentAttempts.Add(attempt);
                context.SaveChanges();
                LoggingService.Information("Exam started. AttemptId={AttemptId} StudentId={StudentId} QuizId={QuizId}", attempt.Id, studentId, quizId);
                return attempt.Id;
            }
        }

        public ExamSessionDto GetExamSession(int studentId, int attemptId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var attempt = context.StudentAttempts
                    .Include(x => x.Quiz.Subject)
                    .Include(x => x.Quiz.Questions.Select(q => q.Choices))
                    .Include(x => x.Answers)
                    .SingleOrDefault(x => x.Id == attemptId && x.StudentId == studentId);

                if (attempt == null)
                {
                    throw new InvalidOperationException("Exam attempt not found.");
                }

                ValidateStudentCanAccessQuiz(attempt.Quiz);

                if (attempt.SubmittedAt.HasValue)
                {
                    throw new InvalidOperationException("This exam attempt has already been submitted.");
                }

                return new ExamSessionDto
                {
                    AttemptId = attempt.Id,
                    QuizId = attempt.QuizId,
                    QuizTitle = attempt.Quiz.Title,
                    SubjectName = attempt.Quiz.Subject != null ? attempt.Quiz.Subject.Name : "Unknown Subject",
                    Topic = attempt.Quiz.Topic,
                    DurationMinutes = attempt.Quiz.DurationMinutes,
                    StartedAtUtc = attempt.StartedAt,
                    Questions = attempt.Quiz.Questions
                        .OrderBy(x => x.OrderIndex)
                        .Select(x => new ExamQuestionDto
                        {
                            Id = x.Id,
                            OrderIndex = x.OrderIndex,
                            Text = x.Text,
                            SelectedChoiceId = attempt.Answers
                                .Where(a => a.QuestionId == x.Id)
                                .Select(a => a.SelectedChoiceId)
                                .FirstOrDefault(),
                            Choices = x.Choices
                                .OrderBy(c => c.OrderIndex)
                                .Select(c => new ExamChoiceDto
                                {
                                    Id = c.Id,
                                    Text = c.Text
                                })
                                .ToList()
                        })
                        .ToList()
                };
            }
        }

        public void SaveAnswer(int studentId, int attemptId, int questionId, int? selectedChoiceId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var attempt = context.StudentAttempts
                    .Include(x => x.Answers)
                    .Include(x => x.Quiz)
                    .SingleOrDefault(x => x.Id == attemptId && x.StudentId == studentId);

                if (attempt == null)
                {
                    throw new InvalidOperationException("Exam attempt not found.");
                }

                if (attempt.SubmittedAt.HasValue)
                {
                    throw new InvalidOperationException("Submitted attempts cannot be changed.");
                }

                var answer = attempt.Answers.SingleOrDefault(x => x.QuestionId == questionId);
                if (answer == null)
                {
                    throw new InvalidOperationException("Question does not belong to this attempt.");
                }

                Choice selectedChoice = null;
                if (selectedChoiceId.HasValue)
                {
                    selectedChoice = context.Choices.SingleOrDefault(x => x.Id == selectedChoiceId.Value && x.QuestionId == questionId);
                    if (selectedChoice == null)
                    {
                        throw new InvalidOperationException("Selected answer is invalid for this question.");
                    }
                }

                answer.SelectedChoiceId = selectedChoiceId;
                answer.IsCorrect = selectedChoice != null && selectedChoice.IsCorrect;
                answer.AnsweredAt = DateTime.UtcNow;
                context.SaveChanges();
            }
        }

        public ExamSubmitResultDto SubmitAttempt(int studentId, int attemptId, bool autoSubmitted)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var attempt = context.StudentAttempts
                    .Include(x => x.Answers)
                    .SingleOrDefault(x => x.Id == attemptId && x.StudentId == studentId);

                if (attempt == null)
                {
                    throw new InvalidOperationException("Exam attempt not found.");
                }

                if (attempt.SubmittedAt.HasValue)
                {
                    throw new InvalidOperationException("This exam attempt has already been submitted.");
                }

                var now = DateTime.UtcNow;
                var totalQuestions = attempt.Answers.Count;
                var answeredQuestions = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue);
                var correctAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && x.IsCorrect);
                var wrongAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && !x.IsCorrect);
                var score = totalQuestions == 0 ? 0D : (double)correctAnswers / totalQuestions * 100D;

                attempt.SubmittedAt = now;
                attempt.TimeSpentSeconds = Math.Max(0, (int)Math.Round((now - attempt.StartedAt).TotalSeconds));
                attempt.ScorePercentage = score;
                context.SaveChanges();

                LoggingService.Information(
                    "Exam submitted. AttemptId={AttemptId} StudentId={StudentId} AutoSubmitted={AutoSubmitted} Score={Score} CorrectAnswers={CorrectAnswers} TotalQuestions={TotalQuestions}",
                    attempt.Id,
                    studentId,
                    autoSubmitted,
                    Math.Round(score, 1),
                    correctAnswers,
                    totalQuestions);

                return new ExamSubmitResultDto
                {
                    AttemptId = attempt.Id,
                    QuizTitle = attempt.Quiz != null ? attempt.Quiz.Title : "Quiz",
                    TotalQuestions = totalQuestions,
                    AnsweredQuestions = answeredQuestions,
                    CorrectAnswers = correctAnswers,
                    WrongAnswers = wrongAnswers,
                    UnansweredQuestions = Math.Max(0, totalQuestions - answeredQuestions),
                    ScorePercentage = score,
                    WasAutoSubmitted = autoSubmitted
                };
            }
        }

        public StudentAttemptSummaryDto GetAttemptSummary(int studentId, int attemptId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var attempt = context.StudentAttempts
                    .Include(x => x.Student)
                    .Include(x => x.Quiz.Subject)
                    .Include(x => x.Answers)
                    .SingleOrDefault(x => x.Id == attemptId && x.StudentId == studentId);

                if (attempt == null)
                {
                    throw new InvalidOperationException("Result summary not found.");
                }

                if (!attempt.SubmittedAt.HasValue)
                {
                    throw new InvalidOperationException("This attempt has not been submitted yet.");
                }

                var answeredQuestions = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue);
                var correctAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && x.IsCorrect);
                var wrongAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && !x.IsCorrect);
                var totalQuestions = attempt.Answers.Count;

                return new StudentAttemptSummaryDto
                {
                    AttemptId = attempt.Id,
                    StudentName = attempt.Student != null ? attempt.Student.Name : "Student",
                    QuizTitle = attempt.Quiz != null ? attempt.Quiz.Title : "Quiz",
                    SubjectName = attempt.Quiz != null && attempt.Quiz.Subject != null ? attempt.Quiz.Subject.Name : "Unknown Subject",
                    Topic = attempt.Quiz != null ? attempt.Quiz.Topic : string.Empty,
                    ScorePercentage = Math.Round(attempt.ScorePercentage ?? 0, 1),
                    TotalQuestions = totalQuestions,
                    AnsweredQuestions = answeredQuestions,
                    CorrectAnswers = correctAnswers,
                    WrongAnswers = wrongAnswers,
                    UnansweredQuestions = Math.Max(0, totalQuestions - answeredQuestions),
                    SubmittedAtDisplay = attempt.SubmittedAt.HasValue ? attempt.SubmittedAt.Value.ToLocalTime().ToString("g") : "Pending",
                    TimeSpentDisplay = FormatDuration(attempt.TimeSpentSeconds),
                    WasAutoSubmitted = attempt.TimeSpentSeconds >= (attempt.Quiz != null ? attempt.Quiz.DurationMinutes * 60 : int.MaxValue)
                };
            }
        }

        private static void EnsureStudent(QuizGenAIDbContext context, int userId)
        {
            var allowed = context.UserRoles.Any(x => x.UserId == userId && x.Role == UserRole.Student);
            if (!allowed)
            {
                throw new InvalidOperationException("Only students can take quizzes.");
            }
        }

        private static void ValidateStudentCanAccessQuiz(Quiz quiz)
        {
            if (quiz.Status != QuizStatus.Published)
            {
                throw new InvalidOperationException("Students can only start published quizzes.");
            }

            var now = DateTime.UtcNow;
            if (quiz.AvailableFrom.HasValue && quiz.AvailableFrom.Value > now)
            {
                throw new InvalidOperationException("This quiz is not available yet.");
            }

            if (quiz.AvailableUntil.HasValue && quiz.AvailableUntil.Value < now)
            {
                throw new InvalidOperationException("This quiz is already closed.");
            }

            if (quiz.Questions == null || !quiz.Questions.Any())
            {
                throw new InvalidOperationException("This quiz has no questions.");
            }
        }

        private static string FormatDuration(int seconds)
        {
            var safeSeconds = Math.Max(0, seconds);
            var timeSpan = TimeSpan.FromSeconds(safeSeconds);
            if (timeSpan.TotalHours >= 1)
            {
                return string.Format("{0:%h}h {0:%m}m {0:%s}s", timeSpan);
            }

            return string.Format("{0:%m}m {0:%s}s", timeSpan);
        }
    }
}
