using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Microsoft.AppCenter.Crashes;
using Time.ViewModels;

namespace Time.Services
{
    public record Settings(WindowProperties Properties, Visuals Visuals, DateTimeConfiguration Configuration, SegmentConfiguration[] Segments);

    public class SettingsService
    {
        private readonly string _settingsFilePath;
        private readonly string _applicationDataFolderPath;
        private readonly Mutex _mutex;

        public SettingsService()
        {
            _applicationDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Environment.ProcessPath) ?? "Time");

            _settingsFilePath = Path.Combine(_applicationDataFolderPath, "Settings.json");

            _mutex = new Mutex(false, "date-time-settings-service");
        }

        public void Save(Settings settings)
        {
            try
            {
                _ = _mutex.WaitOne();

                _ = Directory.CreateDirectory(_applicationDataFolderPath); // Assure that we do have our directory

                File.WriteAllText(_settingsFilePath, JsonSerializer.Serialize(settings));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public Settings Load()
        {
            Settings settings = null;

            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(_settingsFilePath));
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            }

            settings ??= GetDefaults();

            if (settings.Segments is null)
            {
                settings = settings with
                {
                    Segments = GetDefaultSegments()
                };
            }

            return settings;
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
                },
                GetDefaultSegments());
        }

        public SegmentConfiguration[] GetDefaultSegments()
        {
            return new[]
            {
                new SegmentConfiguration
                {
                    Size = 24,
                    Format = "t"
                },
                new SegmentConfiguration
                {
                    Format = "dddd"
                },
                new SegmentConfiguration
                {
                    Format = "d"
                },
            };
        }
    }
}
