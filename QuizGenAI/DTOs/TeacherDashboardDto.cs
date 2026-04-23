using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class TeacherDashboardDto
    {
        public TeacherDashboardDto()
        {
            RecentSubmissions = new List<RecentSubmissionDto>();
            StatusBreakdown = new List<QuizStatusBreakdownDto>();
            SubjectPerformance = new List<SubjectPerformanceDto>();
        }

        public int TotalStudents { get; set; }
        public int TotalQuizzes { get; set; }
        public double AverageScore { get; set; }
        public int PublishedQuizzes { get; set; }
        public int DraftQuizzes { get; set; }
        public int ArchivedQuizzes { get; set; }
        public int SubmittedAttempts { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public List<RecentSubmissionDto> RecentSubmissions { get; set; }
        public List<QuizStatusBreakdownDto> StatusBreakdown { get; set; }
        public List<SubjectPerformanceDto> SubjectPerformance { get; set; }
    }

    public class RecentSubmissionDto
    {
        public string StudentName { get; set; }
        public string QuizTitle { get; set; }
        public double ScorePercentage { get; set; }
        public string SubmittedAtDisplay { get; set; }
    }

    public class QuizStatusBreakdownDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }

    public class SubjectPerformanceDto
    {
        public string SubjectName { get; set; }
        public double AverageScore { get; set; }
        public int AttemptCount { get; set; }
    }
}
