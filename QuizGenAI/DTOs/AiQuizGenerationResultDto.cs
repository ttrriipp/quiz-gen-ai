namespace QuizGenAI.DTOs
{
    public class AiQuizGenerationResultDto
    {
        public QuizEditorDto Quiz { get; set; }
        public string Prompt { get; set; }
        public string RawResponseJson { get; set; }
        public string Provider { get; set; }
        public string ModelName { get; set; }
        public bool UsedFallback { get; set; }
    }
}
