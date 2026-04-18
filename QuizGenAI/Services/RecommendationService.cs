using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizGenAI.Data;
using QuizGenAI.DTOs;

namespace QuizGenAI.Services
{
    public class RecommendationService
    {
        public RecommendationResultDto GetRecommendationsForAttempt(int studentId, int attemptId)
        {
            AttemptRecommendationContext context;

            using (var dbContext = new QuizGenAIDbContext())
            {
                var attempt = dbContext.StudentAttempts
                    .Include(x => x.Student)
                    .Include(x => x.Quiz.Subject)
                    .Include(x => x.Answers.Select(a => a.Question))
                    .SingleOrDefault(x => x.Id == attemptId && x.StudentId == studentId);

                if (attempt == null)
                {
                    throw new InvalidOperationException("Recommendation context was not found.");
                }

                if (!attempt.SubmittedAt.HasValue)
                {
                    throw new InvalidOperationException("Recommendations are available only after submission.");
                }

                context = BuildRecommendationContext(attempt);
            }

            var baseUrl = ConfigurationManager.AppSettings["AiApiBaseUrl"];
            var apiKey = ConfigurationManager.AppSettings["AiApiKey"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                LoggingService.Warning("Recommendation generation is using fallback because no AI API base URL is configured. AttemptId={AttemptId}", attemptId);
                return BuildFallbackRecommendations(context, "Demo Fallback");
            }

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = BuildPrompt(context)
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.6,
                    responseMimeType = "application/json"
                }
            };

            try
            {
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(apiKey))
                    {
                        client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey.Trim());
                    }

                    using (var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"))
                    using (var response = client.PostAsync(baseUrl.Trim(), content).GetAwaiter().GetResult())
                    {
                        var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
                        {
                            LoggingService.Warning("Recommendation generation fell back after AI API error. AttemptId={AttemptId} StatusCode={StatusCode}", attemptId, response.StatusCode);
                            return BuildFallbackRecommendations(context, "Fallback After AI Error");
                        }

                        var recommendations = ParseRecommendations(responseContent);
                        if (recommendations.Count == 0)
                        {
                            LoggingService.Warning("Recommendation generation fell back after empty AI response. AttemptId={AttemptId}", attemptId);
                            return BuildFallbackRecommendations(context, "Fallback After Empty AI Response");
                        }

                        LoggingService.Information("Recommendation generation completed successfully. AttemptId={AttemptId} RecommendationCount={RecommendationCount}", attemptId, recommendations.Count);
                        return new RecommendationResultDto
                        {
                            UsedFallback = false,
                            SourceLabel = "Configured AI API",
                            WeakAreaSummary = context.WeakAreaSummary,
                            Recommendations = recommendations
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Recommendation generation failed and fell back. AttemptId={AttemptId}", attemptId);
                return BuildFallbackRecommendations(context, "Fallback After AI Error");
            }
        }

        private static AttemptRecommendationContext BuildRecommendationContext(Models.StudentAttempt attempt)
        {
            var incorrectQuestions = attempt.Answers
                .Where(x => x.SelectedChoiceId.HasValue && !x.IsCorrect)
                .Select(x => x.Question != null ? x.Question.Text : "Unknown question")
                .Take(3)
                .ToList();

            var unansweredQuestions = attempt.Answers
                .Where(x => !x.SelectedChoiceId.HasValue)
                .Select(x => x.Question != null ? x.Question.Text : "Unknown question")
                .Take(2)
                .ToList();

            var weakSummary = incorrectQuestions.Any() || unansweredQuestions.Any()
                ? string.Format(
                    "Incorrect items: {0}. Unanswered items: {1}.",
                    incorrectQuestions.Any() ? string.Join(" | ", incorrectQuestions) : "none",
                    unansweredQuestions.Any() ? string.Join(" | ", unansweredQuestions) : "none")
                : "The student completed the quiz without incorrect or unanswered items.";

            return new AttemptRecommendationContext
            {
                StudentName = attempt.Student != null ? attempt.Student.Name : "Student",
                QuizTitle = attempt.Quiz != null ? attempt.Quiz.Title : "Quiz",
                SubjectName = attempt.Quiz != null && attempt.Quiz.Subject != null ? attempt.Quiz.Subject.Name : "Unknown Subject",
                Topic = attempt.Quiz != null ? attempt.Quiz.Topic : string.Empty,
                ScorePercentage = Math.Round(attempt.ScorePercentage ?? 0, 1),
                CorrectAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && x.IsCorrect),
                WrongAnswers = attempt.Answers.Count(x => x.SelectedChoiceId.HasValue && !x.IsCorrect),
                UnansweredQuestions = attempt.Answers.Count(x => !x.SelectedChoiceId.HasValue),
                WeakAreaSummary = weakSummary,
                IncorrectQuestions = incorrectQuestions,
                UnansweredQuestionsTexts = unansweredQuestions
            };
        }

        private static string BuildPrompt(AttemptRecommendationContext context)
        {
            return string.Format(
                "Generate study recommendations for a student after a quiz.{0}" +
                "Student: {1}{0}" +
                "Quiz: {2}{0}" +
                "Subject: {3}{0}" +
                "Topic: {4}{0}" +
                "Score: {5:0.#}%{0}" +
                "Correct answers: {6}{0}" +
                "Wrong answers: {7}{0}" +
                "Unanswered questions: {8}{0}" +
                "Weak area summary: {9}{0}{0}" +
                "Return strict JSON only as an array of 3 objects with title and description. " +
                "Keep the advice practical, short, and specific to the weak areas.",
                Environment.NewLine,
                context.StudentName,
                context.QuizTitle,
                context.SubjectName,
                context.Topic,
                context.ScorePercentage,
                context.CorrectAnswers,
                context.WrongAnswers,
                context.UnansweredQuestions,
                context.WeakAreaSummary);
        }

        private static RecommendationResultDto BuildFallbackRecommendations(AttemptRecommendationContext context, string sourceLabel)
        {
            var result = new RecommendationResultDto
            {
                UsedFallback = true,
                SourceLabel = sourceLabel,
                WeakAreaSummary = context.WeakAreaSummary
            };

            result.Recommendations.Add(new StudentRecommendationDto
            {
                Title = string.Format("Review {0} fundamentals", context.SubjectName),
                Description = string.Format("Go back over the key ideas in {0}, especially the topic '{1}', before trying another timed quiz.", context.SubjectName, context.Topic)
            });
            result.Recommendations.Add(new StudentRecommendationDto
            {
                Title = "Rework missed questions",
                Description = context.IncorrectQuestions.Any()
                    ? string.Format("Rewrite the missed concepts in your own words: {0}. Then explain why the correct answer is right.", string.Join("; ", context.IncorrectQuestions))
                    : "Rework one short practice set and explain each answer choice out loud to reinforce recall."
            });
            result.Recommendations.Add(new StudentRecommendationDto
            {
                Title = "Improve quiz readiness",
                Description = context.UnansweredQuestionsTexts.Any()
                    ? string.Format("Spend extra time on the items you skipped: {0}. These gaps usually point to lower confidence or slower recall.", string.Join("; ", context.UnansweredQuestionsTexts))
                    : "Repeat a short timed drill to improve speed while keeping your current accuracy."
            });

            return result;
        }

        private static List<StudentRecommendationDto> ParseRecommendations(string content)
        {
            var token = JToken.Parse(content);
            var array = ResolveRecommendationArray(token);
            if (array == null)
            {
                return new List<StudentRecommendationDto>();
            }

            var items = array.ToObject<List<StudentRecommendationDto>>();
            return items == null
                ? new List<StudentRecommendationDto>()
                : items.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Title) && !string.IsNullOrWhiteSpace(x.Description)).Take(3).ToList();
        }

        private static JArray ResolveRecommendationArray(JToken token)
        {
            if (token == null)
            {
                return null;
            }

            if (token.Type == JTokenType.Array)
            {
                return (JArray)token;
            }

            var objectToken = token as JObject;
            if (objectToken == null)
            {
                return null;
            }

            var direct = objectToken["recommendations"] as JArray;
            if (direct != null)
            {
                return direct;
            }

            var geminiTextToken = objectToken.SelectToken("candidates[0].content.parts[0].text");
            if (geminiTextToken != null && !string.IsNullOrWhiteSpace(geminiTextToken.ToString()))
            {
                return ResolveRecommendationArray(JToken.Parse(geminiTextToken.ToString()));
            }

            var textOutputToken = objectToken.SelectToken("output[0].content[0].text");
            if (textOutputToken != null && !string.IsNullOrWhiteSpace(textOutputToken.ToString()))
            {
                return ResolveRecommendationArray(JToken.Parse(textOutputToken.ToString()));
            }

            return null;
        }

        private class AttemptRecommendationContext
        {
            public string StudentName { get; set; }
            public string QuizTitle { get; set; }
            public string SubjectName { get; set; }
            public string Topic { get; set; }
            public double ScorePercentage { get; set; }
            public int CorrectAnswers { get; set; }
            public int WrongAnswers { get; set; }
            public int UnansweredQuestions { get; set; }
            public string WeakAreaSummary { get; set; }
            public List<string> IncorrectQuestions { get; set; }
            public List<string> UnansweredQuestionsTexts { get; set; }
        }
    }
}
