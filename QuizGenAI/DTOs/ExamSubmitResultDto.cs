namespace QuizGenAI.DTOs
{
    public class ExamSubmitResultDto
    {
        public int AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double ScorePercentage { get; set; }
        public bool WasAutoSubmitted { get; set; }
    }
}
