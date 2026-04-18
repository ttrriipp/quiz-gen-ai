using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;

namespace QuizGenAI.Services
{
    public class AiQuizService
    {
        public async Task<AiQuizGenerationResultDto> GenerateQuizAsync(AiQuizRequestDto request)
        {
            ValidateRequest(request);

            var prompt = BuildPrompt(request);
            var baseUrl = ConfigurationManager.AppSettings["AiApiBaseUrl"];
            var apiKey = ConfigurationManager.AppSettings["AiApiKey"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return BuildFallbackResult(request, prompt);
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
                                text = prompt
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    responseMimeType = "application/json"
                }
            };

            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey.Trim());
                }

                var json = JsonConvert.SerializeObject(payload);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                using (var response = await client.PostAsync(baseUrl.Trim(), content).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
                    {
                        var details = string.IsNullOrWhiteSpace(responseContent)
                            ? "No response body was returned."
                            : responseContent;
                        throw new InvalidOperationException(
                            string.Format("AI generation failed with status code {0}.{1}{1}{2}",
                                response.StatusCode,
                                Environment.NewLine,
                                details));
                    }

                    var normalizedQuestions = ParseQuestions(responseContent);
                    return BuildResultFromQuestions(request, prompt, responseContent, normalizedQuestions, "Configured AI API", "external");
                }
            }
        }

        public string BuildPrompt(AiQuizRequestDto request)
        {
            return string.Format(
                "Generate {0} multiple-choice quiz questions for subject '{1}' about topic '{2}'. " +
                "Difficulty: {3}. Return strict JSON only as an array. " +
                "Each item must contain questionText, choices, correctAnswer, and explanation. " +
                "Each question must have exactly 4 choices and exactly 1 correct answer.",
                request.QuestionCount,
                request.SubjectName,
                request.Topic,
                request.Difficulty);
        }

        private static void ValidateRequest(AiQuizRequestDto request)
        {
            if (request == null)
            {
                throw new InvalidOperationException("AI request data is required.");
            }

            if (request.SubjectId <= 0 || string.IsNullOrWhiteSpace(request.SubjectName))
            {
                throw new InvalidOperationException("Subject is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Topic))
            {
                throw new InvalidOperationException("Topic is required.");
            }

            if (!Enum.IsDefined(typeof(QuizDifficulty), request.Difficulty))
            {
                throw new InvalidOperationException("Difficulty is required.");
            }

            if (request.QuestionCount <= 0)
            {
                throw new InvalidOperationException("Question count must be greater than zero.");
            }
        }

        private AiQuizGenerationResultDto BuildFallbackResult(AiQuizRequestDto request, string prompt)
        {
            var fallbackQuestions = new List<AiQuizQuestionDto>();
            for (var i = 1; i <= request.QuestionCount; i++)
            {
                var correct = string.Format("{0} concept {1}", request.Topic.Trim(), i);
                fallbackQuestions.Add(new AiQuizQuestionDto
                {
                    QuestionText = string.Format("Which option best describes {0} for {1}?", request.Topic.Trim(), request.SubjectName),
                    CorrectAnswer = correct,
                    Explanation = string.Format("This fallback explanation highlights a core point about {0} at {1} difficulty.", request.Topic.Trim(), request.Difficulty),
                    Choices = new List<string>
                    {
                        correct,
                        string.Format("Common misconception about {0}", request.Topic.Trim()),
                        string.Format("Unrelated idea from {0}", request.SubjectName),
                        string.Format("Incorrect detail for item {0}", i)
                    }
                });
            }

            var rawJson = JsonConvert.SerializeObject(fallbackQuestions, Formatting.Indented);
            var result = BuildResultFromQuestions(request, prompt, rawJson, fallbackQuestions, "Demo Fallback", "local");
            result.UsedFallback = true;
            return result;
        }

        private static AiQuizGenerationResultDto BuildResultFromQuestions(
            AiQuizRequestDto request,
            string prompt,
            string rawResponse,
            List<AiQuizQuestionDto> normalizedQuestions,
            string provider,
            string modelName)
        {
            if (normalizedQuestions == null || normalizedQuestions.Count == 0)
            {
                throw new InvalidOperationException("The AI response did not contain any questions.");
            }

            var quiz = new QuizEditorDto
            {
                Title = string.Format("{0} - {1}", request.SubjectName, request.Topic.Trim()),
                SubjectId = request.SubjectId,
                Topic = request.Topic.Trim(),
                Difficulty = request.Difficulty,
                DurationMinutes = request.DurationMinutes <= 0 ? Math.Max(10, request.QuestionCount * 2) : request.DurationMinutes,
                Status = QuizStatus.Draft,
                IsAiGenerated = true,
                AiGenerationPrompt = prompt,
                AiGenerationRawResponseJson = rawResponse,
                AiGenerationProvider = provider,
                AiGenerationModelName = modelName
            };

            foreach (var question in normalizedQuestions)
            {
                var normalizedChoices = question.Choices
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Take(4)
                    .ToList();

                if (normalizedChoices.Count < 4)
                {
                    throw new InvalidOperationException("Each AI-generated question must contain four choices.");
                }

                var correctChoice = normalizedChoices.FirstOrDefault(x =>
                    string.Equals(x.Trim(), (question.CorrectAnswer ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrWhiteSpace(correctChoice))
                {
                    throw new InvalidOperationException("The AI response contains a correct answer that does not match its choices.");
                }

                var quizQuestion = new QuizQuestionEditorDto
                {
                    Text = question.QuestionText.Trim(),
                    Explanation = string.IsNullOrWhiteSpace(question.Explanation) ? null : question.Explanation.Trim()
                };

                foreach (var choice in normalizedChoices)
                {
                    quizQuestion.Choices.Add(new QuizChoiceEditorDto
                    {
                        Text = choice.Trim(),
                        IsCorrect = string.Equals(choice.Trim(), correctChoice.Trim(), StringComparison.OrdinalIgnoreCase)
                    });
                }

                quiz.Questions.Add(quizQuestion);
            }

            return new AiQuizGenerationResultDto
            {
                Quiz = quiz,
                Prompt = prompt,
                RawResponseJson = rawResponse,
                Provider = provider,
                ModelName = modelName
            };
        }

        private static List<AiQuizQuestionDto> ParseQuestions(string content)
        {
            var parsedToken = JToken.Parse(content);
            var questionArray = ResolveQuestionArray(parsedToken);
            if (questionArray == null)
            {
                throw new InvalidOperationException("The AI response could not be parsed into quiz questions.");
            }

            var questions = questionArray.ToObject<List<AiQuizQuestionDto>>();
            if (questions == null || questions.Count == 0)
            {
                throw new InvalidOperationException("The AI response returned an empty question list.");
            }

            return questions;
        }

        private static JArray ResolveQuestionArray(JToken token)
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

            var directQuestions = objectToken["questions"] as JArray;
            if (directQuestions != null)
            {
                return directQuestions;
            }

            var contentToken = objectToken.SelectToken("choices[0].message.content");
            if (contentToken != null && !string.IsNullOrWhiteSpace(contentToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(contentToken.ToString()));
            }

            var geminiTextToken = objectToken.SelectToken("candidates[0].content.parts[0].text");
            if (geminiTextToken != null && !string.IsNullOrWhiteSpace(geminiTextToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(geminiTextToken.ToString()));
            }

            var textOutputToken = objectToken.SelectToken("output[0].content[0].text");
            if (textOutputToken != null && !string.IsNullOrWhiteSpace(textOutputToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(textOutputToken.ToString()));
            }

            return null;
        }
    }
}
