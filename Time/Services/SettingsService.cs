using System;
using System.IO;
using System.Text.Json;
using Time.ViewModels;

namespace Time.Services
{
    public record Settings(WindowProperties Properties, Visuals Visuals, DateTimeConfiguration Configuration);

    public class SettingsService
    {
        private readonly string _settingsFilePath;
        private readonly string _applicationDataFolderPath;

        public SettingsService()
        {
            _applicationDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Environment.ProcessPath) ?? "Time");

            _settingsFilePath = Path.Combine(_applicationDataFolderPath, "Settings.json");
        }

        public void Save(Settings settings)
        {
            _ = Directory.CreateDirectory(_applicationDataFolderPath); // Assure that we do have our directory

            File.WriteAllText(_settingsFilePath, JsonSerializer.Serialize(settings));
        }

        public Settings Load()
        {
            Settings settings = null;

            if(File.Exists(_settingsFilePath))
            {
                try
                {
                    settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(_settingsFilePath));
                }
                catch (Exception)
                {
                    // Yeah nah
                }
            }

            return settings ?? GetDefaults();
        }

        public Settings GetDefaults()
        {
            return new Settings(
                new WindowProperties(),
                new Visuals
                {
                    Opacity = 0.5
                },
                new DateTimeConfiguration
                {
                    IsShortTime = true,
                    ShowDate = true
                });
        }
    }
}
