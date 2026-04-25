using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using chat.Models;

namespace chat.Services
{
    public class MemoryService
    {
        private readonly JsonStorage storage;
        private readonly string memoryPath;
        private readonly string historyPath;

        public MemoryService(JsonStorage storage)
        {
            this.storage = storage;

            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CodexLocalChat");

            memoryPath = Path.Combine(folder, "memory.json");
            historyPath = Path.Combine(folder, "history.json");
        }

        public AssistantMemory LoadMemory()
        {
            return storage.Load(memoryPath, new AssistantMemory());
        }

        public void SaveMemory(AssistantMemory memory)
        {
            storage.Save(memoryPath, memory);
        }

        public List<ConversationTurn> LoadHistory()
        {
            return storage.Load(historyPath, new List<ConversationTurn>());
        }

        public void AppendHistory(string role, string message)
        {
            List<ConversationTurn> history = LoadHistory();
            history.Add(new ConversationTurn
            {
                Role = role,
                Message = message,
                Timestamp = DateTime.Now
            });

            List<ConversationTurn> trimmed = history
                .OrderByDescending(item => item.Timestamp)
                .Take(40)
                .OrderBy(item => item.Timestamp)
                .ToList();

            storage.Save(historyPath, trimmed);
        }

        public void ClearHistory()
        {
            storage.Save(historyPath, new List<ConversationTurn>());
        }
    }
}
