using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using QuizGenAI.Data;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;

namespace QuizGenAI.Services
{
    public class ReportService
    {
        public TeacherReportsDto GetTeacherReports()
        {
            using (var context = new QuizGenAIDbContext())
            {
                var submittedAttempts = context.StudentAttempts
                    .Include(x => x.Student)
                    .Include(x => x.Quiz.Subject)
                    .Include(x => x.Answers.Select(a => a.Question))
                    .Where(x => x.SubmittedAt.HasValue)
                    .ToList();

                var reports = new TeacherReportsDto
                {
                    AverageScore = submittedAttempts.Count == 0 ? 0 : Math.Round(submittedAttempts.Average(x => x.ScorePercentage ?? 0), 1),
                    PassCount = submittedAttempts.Count(x => (x.ScorePercentage ?? 0) >= 75),
                    FailCount = submittedAttempts.Count(x => (x.ScorePercentage ?? 0) < 75)
                };

                reports.SubjectPerformance = submittedAttempts
                    .Where(x => x.Quiz != null && x.Quiz.Subject != null)
                    .GroupBy(x => x.Quiz.Subject.Name)
                    .Select(x => new SubjectPerformanceDto
                    {
                        SubjectName = x.Key,
                        AverageScore = Math.Round(x.Average(y => y.ScorePercentage ?? 0), 1),
                        AttemptCount = x.Count()
                    })
                    .OrderByDescending(x => x.AverageScore)
                    .Take(6)
                    .ToList();

                var today = DateTime.Today;
                var endMonth = new DateTime(today.Year, today.Month, 1);
                for (var i = 5; i >= 0; i--)
                {
                    var monthStart = endMonth.AddMonths(-i);
                    var monthEnd = monthStart.AddMonths(1);
                    var inMonth = submittedAttempts
                        .Where(x => x.SubmittedAt.HasValue
                            && x.SubmittedAt.Value >= monthStart
                            && x.SubmittedAt.Value < monthEnd)
                        .ToList();
                    double? monthAvg = inMonth.Count == 0
                        ? (double?)null
                        : Math.Round(inMonth.Average(x => x.ScorePercentage ?? 0), 1);
                    reports.ScoreTrendByMonth.Add(new ScoreTrendMonthDto
                    {
                        MonthLabel = monthStart.ToString("MMM", CultureInfo.InvariantCulture),
                        AverageScore = monthAvg
                    });
                }

                reports.RecentSubmissions = submittedAttempts
                    .OrderByDescending(x => x.SubmittedAt)
                    .Take(8)
                    .Select(x => new RecentSubmissionDto
                    {
                        StudentName = x.Student != null ? x.Student.Name : "Unknown Student",
                        QuizTitle = x.Quiz != null ? x.Quiz.Title : "Unknown Quiz",
                        ScorePercentage = Math.Round(x.ScorePercentage ?? 0, 1),
                        SubmittedAtDisplay = x.SubmittedAt.HasValue ? FormatStoredLocalForDisplay(x.SubmittedAt.Value) : "Pending"
                    })
                    .ToList();

                reports.HardestQuestions = submittedAttempts
                    .SelectMany(x => x.Answers
                        .Where(a => a.Question != null)
                        .Select(a => new
                        {
                            QuizTitle = x.Quiz != null ? x.Quiz.Title : "Quiz",
                            SubjectName = x.Quiz != null && x.Quiz.Subject != null ? x.Quiz.Subject.Name : "Unknown Subject",
                            QuestionText = a.Question.Text,
                            IsCorrect = a.SelectedChoiceId.HasValue && a.IsCorrect
                        }))
                    .GroupBy(x => new { x.QuizTitle, x.SubjectName, x.QuestionText })
                    .Select(x => new HardestQuestionDto
                    {
                        QuizTitle = x.Key.QuizTitle,
                        SubjectName = x.Key.SubjectName,
                        QuestionText = x.Key.QuestionText,
                        Attempts = x.Count(),
                        CorrectRate = Math.Round(x.Average(y => y.IsCorrect ? 100D : 0D), 1)
                    })
                    .OrderBy(x => x.CorrectRate)
                    .ThenByDescending(x => x.Attempts)
                    .Take(6)
                    .ToList();

                return reports;
            }
        }

        public StudentResultsDto GetStudentResults(int studentId)
        {
            using (var context = new QuizGenAIDbContext())
            {
                var student = context.Users.SingleOrDefault(x => x.Id == studentId);

                var attempts = context.StudentAttempts
                    .Include(x => x.Quiz.Subject)
                    .Include(x => x.Answers)
                    .Where(x => x.StudentId == studentId && x.SubmittedAt.HasValue)
                    .OrderByDescending(x => x.SubmittedAt)
                    .ToList();

                var dto = new StudentResultsDto
                {
                    StudentName = student != null ? student.Name : "Student",
                    AverageScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Average(x => x.ScorePercentage ?? 0), 1),
                    QuizzesTaken = attempts.Count,
                    BestScore = attempts.Count == 0 ? 0 : Math.Round(attempts.Max(x => x.ScorePercentage ?? 0), 1),
                    LatestScore = attempts.Count == 0 ? 0 : Math.Round(attempts.First().ScorePercentage ?? 0, 1)
                };

                dto.History = attempts
                    .Take(12)
                    .Select(x =>
                    {
                        var answeredQuestions = x.Answers.Count(a => a.SelectedChoiceId.HasValue);
                        var correctAnswers = x.Answers.Count(a => a.SelectedChoiceId.HasValue && a.IsCorrect);
                        var wrongAnswers = x.Answers.Count(a => a.SelectedChoiceId.HasValue && !a.IsCorrect);
                        var totalQuestions = x.Answers.Count;

                        return new StudentResultHistoryItemDto
                        {
                            AttemptId = x.Id,
                            QuizTitle = x.Quiz != null ? x.Quiz.Title : "Quiz",
                            SubjectName = x.Quiz != null && x.Quiz.Subject != null ? x.Quiz.Subject.Name : "Unknown Subject",
                            ScorePercentage = Math.Round(x.ScorePercentage ?? 0, 1),
                            CorrectAnswers = correctAnswers,
                            WrongAnswers = wrongAnswers,
                            UnansweredQuestions = Math.Max(0, totalQuestions - answeredQuestions),
                            SubmittedAtLocal = x.SubmittedAt.HasValue ? (DateTime?)NormalizeStoredLocal(x.SubmittedAt.Value) : null,
                            SubmittedAtDisplay = x.SubmittedAt.HasValue ? FormatStoredLocalForDisplay(x.SubmittedAt.Value) : "Pending",
                            TimeSpentDisplay = FormatDuration(x.TimeSpentSeconds)
                        };
                    })
                    .ToList();

                return dto;
            }
        }

        public TeacherDashboardDto GetTeacherDashboard()
        {
            using (var context = new QuizGenAIDbContext())
            {
                var submittedAttempts = context.StudentAttempts
                    .Include(x => x.Student)
                    .Include(x => x.Quiz.Subject)
                    .Where(x => x.SubmittedAt.HasValue)
                    .ToList();

                var quizzes = context.Quizzes
                    .Include(x => x.Subject)
                    .Where(x => !x.IsDeleted)
                    .ToList();

                var studentCount = context.UserRoles.Count(x => x.Role == UserRole.Student);

                var dashboard = new TeacherDashboardDto
                {
                    TotalStudents = studentCount,
                    TotalQuizzes = quizzes.Count,
                    AverageScore = submittedAttempts.Count == 0 ? 0 : Math.Round(submittedAttempts.Average(x => x.ScorePercentage ?? 0), 1),
                    PublishedQuizzes = quizzes.Count(x => x.Status == QuizStatus.Published),
                    DraftQuizzes = quizzes.Count(x => x.Status == QuizStatus.Draft),
                    ArchivedQuizzes = quizzes.Count(x => x.Status == QuizStatus.Archived),
                    SubmittedAttempts = submittedAttempts.Count
                };

                dashboard.RecentSubmissions = submittedAttempts
                    .OrderByDescending(x => x.SubmittedAt)
                    .Take(5)
                    .Select(x => new RecentSubmissionDto
                    {
                        StudentName = x.Student != null ? x.Student.Name : "Unknown Student",
                        QuizTitle = x.Quiz != null ? x.Quiz.Title : "Unknown Quiz",
                        ScorePercentage = Math.Round(x.ScorePercentage ?? 0, 1),
                        SubmittedAtDisplay = x.SubmittedAt.HasValue ? FormatStoredLocalForDisplay(x.SubmittedAt.Value) : "Pending"
                    })
                    .ToList();

                dashboard.StatusBreakdown.Add(new QuizStatusBreakdownDto { Label = "Draft", Count = dashboard.DraftQuizzes });
                dashboard.StatusBreakdown.Add(new QuizStatusBreakdownDto { Label = "Published", Count = dashboard.PublishedQuizzes });
                dashboard.StatusBreakdown.Add(new QuizStatusBreakdownDto { Label = "Archived", Count = dashboard.ArchivedQuizzes });

                dashboard.SubjectPerformance = submittedAttempts
                    .Where(x => x.Quiz != null && x.Quiz.Subject != null)
                    .GroupBy(x => x.Quiz.Subject.Name)
                    .Select(x => new SubjectPerformanceDto
                    {
                        SubjectName = x.Key,
                        AverageScore = Math.Round(x.Average(y => y.ScorePercentage ?? 0), 1),
                        AttemptCount = x.Count()
                    })
                    .OrderByDescending(x => x.AverageScore)
                    .Take(5)
                    .ToList();

                return dashboard;
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

        private static DateTime NormalizeStoredLocal(DateTime value)
        {
            return value.Kind == DateTimeKind.Local
                ? value
                : DateTime.SpecifyKind(value, DateTimeKind.Local);
        }

        private static string FormatStoredLocalForDisplay(DateTime value)
        {
            return NormalizeStoredLocal(value).ToString("g");
        }
    }
}
