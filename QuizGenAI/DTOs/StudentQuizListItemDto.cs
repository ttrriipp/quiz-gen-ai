using System;
using QuizGenAI.Enums;

namespace QuizGenAI.DTOs
{
    public class StudentQuizListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public QuizDifficulty Difficulty { get; set; }
        public int DurationMinutes { get; set; }
        public int QuestionCount { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public bool CanStart { get; set; }
        public string AvailabilityLabel { get; set; }
        public int AttemptCount { get; set; }
        public bool HasCompletedAttempt { get; set; }
        public double? BestScore { get; set; }
    }
}
