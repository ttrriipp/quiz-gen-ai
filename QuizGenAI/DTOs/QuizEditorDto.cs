using System.Collections.Generic;
using System;
using QuizGenAI.Enums;

namespace QuizGenAI.DTOs
{
    public class QuizEditorDto
    {
        public QuizEditorDto()
        {
            Questions = new List<QuizQuestionEditorDto>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }
        public string Topic { get; set; }
        public QuizDifficulty Difficulty { get; set; }
        public int DurationMinutes { get; set; }
        public QuizStatus Status { get; set; }
        public bool IsAiGenerated { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public string AiGenerationPrompt { get; set; }
        public string AiGenerationRawResponseJson { get; set; }
        public string AiGenerationProvider { get; set; }
        public string AiGenerationModelName { get; set; }
        public List<QuizQuestionEditorDto> Questions { get; set; }
    }

    public class QuizQuestionEditorDto
    {
        public QuizQuestionEditorDto()
        {
            Choices = new List<QuizChoiceEditorDto>();
        }

        public string Text { get; set; }
        public string Explanation { get; set; }
        public List<QuizChoiceEditorDto> Choices { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public class QuizChoiceEditorDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
