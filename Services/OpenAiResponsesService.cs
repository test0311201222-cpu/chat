using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using chat.Models;

namespace chat.Services
{
    public class OpenAiResponsesService
    {
        private readonly JsonStorage storage;
        private readonly ConfigService configService;

        public OpenAiResponsesService(JsonStorage storage, ConfigService configService)
        {
            this.storage = storage;
            this.configService = configService;
        }

        public bool IsConfigured()
        {
            AppSettings settings = configService.Load();
            return !string.IsNullOrWhiteSpace(settings.OpenAiApiKey) ||
                   !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        }

        public string GetModelName()
        {
            AppSettings settings = configService.Load();
            if (!string.IsNullOrWhiteSpace(settings.OpenAiModel))
            {
                return settings.OpenAiModel;
            }

            string model = Environment.GetEnvironmentVariable("OPENAI_MODEL");
            return string.IsNullOrWhiteSpace(model) ? "gpt-5-mini" : model;
        }

        public async Task<string> GenerateAsync(string instructions, string input)
        {
            AppSettings settings = configService.Load();
            string apiKey = string.IsNullOrWhiteSpace(settings.OpenAiApiKey)
                ? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                : settings.OpenAiApiKey;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("OPENAI_API_KEY nao configurada.");
            }

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(45);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

                var payload = new
                {
                    model = GetModelName(),
                    instructions = instructions,
                    input = input
                };

                string json = storage.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/responses", content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Falha ao chamar a API: " + response.StatusCode + " - " + responseJson);
                }

                return ExtractText(responseJson);
            }
        }

        private string ExtractText(string json)
        {
            object root = storage.DeserializeDynamic(json);
            Dictionary<string, object> rootMap = root as Dictionary<string, object>;

            if (rootMap == null)
            {
                return string.Empty;
            }

            if (rootMap.ContainsKey("output_text"))
            {
                return Convert.ToString(rootMap["output_text"]);
            }

            if (!rootMap.ContainsKey("output"))
            {
                return string.Empty;
            }

            object[] output = rootMap["output"] as object[];
            if (output == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            foreach (object item in output)
            {
                Dictionary<string, object> outputItem = item as Dictionary<string, object>;
                if (outputItem == null || !outputItem.ContainsKey("content"))
                {
                    continue;
                }

                object[] contents = outputItem["content"] as object[];
                if (contents == null)
                {
                    continue;
                }

                foreach (object content in contents)
                {
                    Dictionary<string, object> contentItem = content as Dictionary<string, object>;
                    if (contentItem == null)
                    {
                        continue;
                    }

                    if (contentItem.ContainsKey("text"))
                    {
                        builder.AppendLine(Convert.ToString(contentItem["text"]));
                    }
                }
            }

            return builder.ToString().Trim();
        }
    }
}
