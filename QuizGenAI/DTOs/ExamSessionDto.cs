using System;
using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class ExamSessionDto
    {
        public ExamSessionDto()
        {
            Questions = new List<ExamQuestionDto>();
        }

        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public List<ExamQuestionDto> Questions { get; set; }
    }
}
