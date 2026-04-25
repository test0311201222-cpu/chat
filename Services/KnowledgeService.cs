using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                Name = "Codex Local",
                Role = "Assistente",
                Tone = "claro e prestativo",
                Goal = "Ajudar no estudo",
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
            string text = (message ?? string.Empty).ToLowerInvariant();
            string currentMode = (mode ?? string.Empty).ToLowerInvariant();
            List<KnowledgeItem> items = LoadKnowledge();

            return items
                .Where(item =>
                    item.Keywords.Any(keyword => text.Contains(keyword.ToLowerInvariant())) ||
                    (!string.IsNullOrWhiteSpace(item.Category) && item.Category.ToLowerInvariant() == currentMode))
                .Take(3)
                .ToList();
        }
    }
}
