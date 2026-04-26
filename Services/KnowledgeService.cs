using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using chat.Models;

namespace chat.Services
{
    public class KnowledgeService
    {
        private readonly JsonStorage storage;
        private readonly string profilePath;
        private readonly string knowledgePath;

        public KnowledgeService(JsonStorage storage)
        {
            this.storage = storage;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            profilePath = Path.Combine(basePath, "Data", "assistant-profile.json");
            knowledgePath = Path.Combine(basePath, "Data", "knowledge-base.json");
        }

        public AssistantProfile LoadProfile()
        {
            return storage.Load(profilePath, new AssistantProfile
            {
                Name = "Nira",
                Role = "assistente pessoal local",
                Tone = "humano e claro",
                Goal = "ajudar o usuario em conversa geral, estudo e projetos",
                Rules = new List<string>()
            });
        }

        public List<KnowledgeItem> LoadKnowledge()
        {
            return storage.Load(knowledgePath, new List<KnowledgeItem>());
        }

        public void SaveKnowledge(List<KnowledgeItem> items)
        {
            storage.Save(knowledgePath, items ?? new List<KnowledgeItem>());
        }

        public List<KnowledgeItem> Search(string message, string mode)
        {
            string text = Normalize(message);
            string currentMode = Normalize(mode);
            List<KnowledgeItem> items = LoadKnowledge();

            return items
                .Select(item => new
                {
                    Item = item,
                    Score = GetScore(item, text, currentMode)
                })
                .Where(result => result.Score > 0)
                .OrderByDescending(result => result.Score)
                .ThenBy(result => result.Item.Title)
                .Take(4)
                .Select(result => result.Item)
                .ToList();
        }

        private int GetScore(KnowledgeItem item, string text, string currentMode)
        {
            int score = 0;
            string title = Normalize(item.Title);
            string category = Normalize(item.Category);

            if (title != string.Empty && (text == title || text.Contains(title)))
            {
                score += 7;
            }

            foreach (string keyword in item.Keywords)
            {
                string normalizedKeyword = Normalize(keyword);

                if (normalizedKeyword == string.Empty)
                {
                    continue;
                }

                if (text == normalizedKeyword)
                {
                    score += 8;
                }
                else if (text.Contains(normalizedKeyword))
                {
                    score += normalizedKeyword.Contains(" ") ? 5 : 3;
                }
            }

            if (category == currentMode && currentMode != string.Empty && currentMode != "geral")
            {
                score += 1;
            }

            return score;
        }

        private string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char character in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
