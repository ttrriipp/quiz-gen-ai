using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class StudentResultsDto
    {
        public StudentResultsDto()
        {
            History = new List<StudentResultHistoryItemDto>();
        }

        public string StudentName { get; set; }
        public double AverageScore { get; set; }
        public int QuizzesTaken { get; set; }
        public double BestScore { get; set; }
        public double LatestScore { get; set; }
        public List<StudentResultHistoryItemDto> History { get; set; }
    }

    public class StudentResultHistoryItemDto
    {
        public int AttemptId { get; set; }
        public string QuizTitle { get; set; }
        public string SubjectName { get; set; }
        public double ScorePercentage { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int UnansweredQuestions { get; set; }
        public string SubmittedAtDisplay { get; set; }
        public string TimeSpentDisplay { get; set; }
    }
}
