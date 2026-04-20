using System.Collections.Generic;

namespace QuizGenAI.DTOs
{
    public class RecommendationResultDto
    {
        public RecommendationResultDto()
        {
            Recommendations = new List<StudentRecommendationDto>();
        }

        public bool UsedFallback { get; set; }
        public string SourceLabel { get; set; }
        public string WeakAreaSummary { get; set; }
        public List<StudentRecommendationDto> Recommendations { get; set; }
    }

    public class StudentRecommendationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
