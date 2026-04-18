using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class AiQuizQuestionDto
    {
        public AiQuizQuestionDto()
        {
            Choices = new List<string>();
        }

        public string QuestionText { get; set; }
        public List<string> Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }
}
