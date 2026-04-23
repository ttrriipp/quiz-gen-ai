using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class StudentAttemptReviewDto
    {
        public StudentAttemptReviewDto()
        {
            Questions = new List<StudentAttemptReviewQuestionDto>();
        }

        public int AttemptId { get; set; }
        public string QuizTitle { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public double ScorePercentage { get; set; }
        public string SubmittedAtDisplay { get; set; }
        public List<StudentAttemptReviewQuestionDto> Questions { get; set; }
    }

    public class StudentAttemptReviewQuestionDto
    {
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
        public string QuestionText { get; set; }
        public int? SelectedChoiceId { get; set; }
        public int? CorrectChoiceId { get; set; }
        public List<StudentAttemptReviewChoiceDto> Choices { get; set; }
    }

    public class StudentAttemptReviewChoiceDto
    {
        public int ChoiceId { get; set; }
        public string Text { get; set; }
    }
}
