using System;
using System.Data.Entity;
using System.Linq;
using QuizGenAI.Data;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;

namespace QuizGenAI.Services
{
    public class ReportService
    {
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
                        SubmittedAtDisplay = x.SubmittedAt.HasValue ? x.SubmittedAt.Value.ToLocalTime().ToString("g") : "Pending"
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
                        AverageScore = Math.Round(x.Average(y => y.ScorePercentage ?? 0), 1)
                    })
                    .OrderByDescending(x => x.AverageScore)
                    .Take(5)
                    .ToList();

                return dashboard;
            }
        }
    }
}
