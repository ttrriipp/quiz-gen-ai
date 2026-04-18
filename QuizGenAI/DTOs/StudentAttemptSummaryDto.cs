namespace QuizGenAI.DTOs
{
    public class StudentAttemptSummaryDto
    {
        public int AttemptId { get; set; }
        public string StudentName { get; set; }
        public string QuizTitle { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public double ScorePercentage { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int UnansweredQuestions { get; set; }
        public string SubmittedAtDisplay { get; set; }
        public string TimeSpentDisplay { get; set; }
        public bool WasAutoSubmitted { get; set; }
    }
}
