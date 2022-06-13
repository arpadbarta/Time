using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using Time.Services;

namespace Time.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string _time;
        private string _date;
        private Color _background;
        private Color _foreground;
        private FontFamily _fontFamily;
        private string _day;
        private readonly SettingsService _settingService;
        private Settings _settings;

        public string Time
        {
            get => _time;
            private set => Set(ref _time, value);
        }

        public string Date
        {
            get => _date;
            private set => Set(ref _date, value);
        }

        public string Day
        {
            get => _day;
            set => Set(ref _day, value);
        }
        
        public Color Background
        {
            get => _background;
            set => Set(ref _background, value);
        }

        public Color Foreground
        {
            get => _foreground;
            set => Set(ref _foreground, value);
        }

        public FontFamily FontFamily
        {
            get => _fontFamily;
            set => Set(ref _fontFamily, value);
        }

        public FontFamily SystemFontFamily { get; set; }

        public IReadOnlyList<Color> DefinedColors { get; }
        public IEnumerable<FontFamily> FontCollection => Fonts.SystemFontFamilies.OrderBy(x => x.ToString());
        
        public Settings Settings
        {
            get => _settings;

            set => Set(ref _settings, value);
        }

        public MainViewModel()
        {
            DefinedColors = typeof(Brushes).GetProperties().Select(x => ((SolidColorBrush)x.GetValue(null))!.Color).ToList();

            _settingService = new SettingsService();

            Settings = _settingService.Load();
            Settings.Configuration.PropertyChanged += OnConfigurationChanged;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += OnTick;
            timer.Start();
        }

        public void Load()
        {
            Settings = _settingService.Load();

            UpdateVisuals();

            UpdateDateTime();
        }

        public void Save()
        {
            Settings.Visuals.Background = Background.ToString();
            Settings.Visuals.Foreground = Foreground.ToString();
            Settings.Visuals.FontFamily = FontFamily.ToString();

            _settingService.Save(Settings);
        }

        public void ResetSettings()
        {
            Settings.Configuration.PropertyChanged -= OnConfigurationChanged;

            Settings = _settingService.GetDefaults();

            UpdateVisuals();

            RaisePropertyChanged();

            Settings.Configuration.PropertyChanged += OnConfigurationChanged;
        }

        private void UpdateVisuals()
        {
            FontFamily = FontCollection.FirstOrDefault(x => x.ToString() == Settings.Visuals.FontFamily) ?? SystemFontFamily;
            Background = (Color)(ColorConverter.ConvertFromString(Settings.Visuals.Background) ?? Colors.Black);
            Foreground = (Color)(ColorConverter.ConvertFromString(Settings.Visuals.Foreground) ?? Colors.White);
        }

        private void OnTick(object sender, EventArgs e) => UpdateDateTime();
        
        private void OnConfigurationChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            UpdateDateTime();

        private void UpdateDateTime()
        {
            var now = DateTime.Now;

            Time = _settings.Configuration.IsShortTime ? now.ToShortTimeString() : now.ToLongTimeString();
            Date = _settings.Configuration.IsShortDate ? now.ToShortDateString() : now.ToLongDateString();
            Day = now.ToString("dddd"); // TODO: At some point we should really expose configurable formatting
        }
    }
}
