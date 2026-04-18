using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class ExamQuestionDto
    {
        public ExamQuestionDto()
        {
            Choices = new List<ExamChoiceDto>();
        }

        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public string Text { get; set; }
        public int? SelectedChoiceId { get; set; }
        public List<ExamChoiceDto> Choices { get; set; }
    }
}
