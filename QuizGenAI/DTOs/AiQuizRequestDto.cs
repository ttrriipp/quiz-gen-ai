using QuizGenAI.Enums;

namespace QuizGenAI.DTOs
{
    public class AiQuizRequestDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public string SourceDocumentFileName { get; set; }
        public string SourceDocumentText { get; set; }
        public QuizDifficulty Difficulty { get; set; }
        public int QuestionCount { get; set; }
        public int DurationMinutes { get; set; }
    }
}
