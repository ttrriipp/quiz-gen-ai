using System;
using System.Configuration;

namespace QuizGenAI.Helpers
{
    internal static class AiApiConfiguration
    {
        internal const string BaseUrlSettingKey = "AiApiBaseUrl";
        internal const string ApiKeySettingKey = "AiApiKey";
        internal const string BaseUrlEnvironmentKey = "QUIZGENAI_AI_API_BASE_URL";
        internal const string ApiKeyEnvironmentKey = "QUIZGENAI_AI_API_KEY";
        internal const string GeminiApiKeyEnvironmentKey = "GEMINI_API_KEY";

        internal static string GetBaseUrl()
        {
            return GetValue(BaseUrlSettingKey, BaseUrlEnvironmentKey);
        }

        internal static string GetApiKey()
        {
            return GetValue(ApiKeySettingKey, ApiKeyEnvironmentKey, GeminiApiKeyEnvironmentKey);
        }

        internal static string GetSafeResponseDetail(string responseContent)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return "No response body was returned.";
            }

            var detail = responseContent.Trim();
            var apiKey = GetApiKey();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                detail = detail.Replace(apiKey, "[redacted-api-key]");
            }

            return detail.Length <= 800 ? detail : detail.Substring(0, 800) + "...";
        }

        private static string GetValue(string appSettingKey, params string[] environmentKeys)
        {
            foreach (var environmentKey in environmentKeys)
            {
                var environmentValue = GetEnvironmentValue(environmentKey);
                if (!string.IsNullOrWhiteSpace(environmentValue))
                {
                    return environmentValue.Trim();
                }
            }

            var appSettingValue = ConfigurationManager.AppSettings[appSettingKey];
            if (!string.IsNullOrWhiteSpace(appSettingValue))
            {
                return appSettingValue.Trim();
            }

            return null;
        }

        private static string GetEnvironmentValue(string environmentKey)
        {
            var processValue = Environment.GetEnvironmentVariable(environmentKey);
            if (!string.IsNullOrWhiteSpace(processValue))
            {
                return processValue;
            }

            var userValue = Environment.GetEnvironmentVariable(environmentKey, EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(userValue))
            {
                return userValue;
            }

            var machineValue = Environment.GetEnvironmentVariable(environmentKey, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(machineValue))
            {
                return machineValue;
            }

            return null;
        }
    }
}
