using System;
using System.IO;
using chat.Models;

namespace chat.Services
{
    public class ConfigService
    {
        private readonly JsonStorage storage;
        private readonly string settingsPath;

        public ConfigService(JsonStorage storage)
        {
            this.storage = storage;
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CodexLocalChat");
            settingsPath = Path.Combine(folder, "settings.json");
        }

        public AppSettings Load()
        {
            return storage.Load(settingsPath, new AppSettings());
        }

        public void Save(AppSettings settings)
        {
            storage.Save(settingsPath, settings);
        }
    }
}
