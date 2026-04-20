using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class TeacherReportsDto
    {
        public TeacherReportsDto()
        {
            SubjectPerformance = new List<SubjectPerformanceDto>();
            HardestQuestions = new List<HardestQuestionDto>();
            RecentSubmissions = new List<RecentSubmissionDto>();
            ScoreTrendByMonth = new List<ScoreTrendMonthDto>();
        }

        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public double AverageScore { get; set; }
        public List<ScoreTrendMonthDto> ScoreTrendByMonth { get; set; }
        public List<SubjectPerformanceDto> SubjectPerformance { get; set; }
        public List<HardestQuestionDto> HardestQuestions { get; set; }
        public List<RecentSubmissionDto> RecentSubmissions { get; set; }
    }

    public class ScoreTrendMonthDto
    {
        public string MonthLabel { get; set; }
        public double? AverageScore { get; set; }
    }

    public class HardestQuestionDto
    {
        public string QuizTitle { get; set; }
        public string SubjectName { get; set; }
        public string QuestionText { get; set; }
        public double CorrectRate { get; set; }
        public int Attempts { get; set; }
    }
}
