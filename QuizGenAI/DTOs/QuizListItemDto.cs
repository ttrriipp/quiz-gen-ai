using System;
using QuizGenAI.Enums;

namespace QuizGenAI.DTOs
{
    public class QuizListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public QuizDifficulty Difficulty { get; set; }
        public QuizStatus Status { get; set; }
        public int DurationMinutes { get; set; }
        public int QuestionCount { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool CanPublish { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
    }
}
