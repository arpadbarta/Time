using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Time.Services;

namespace Time.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private Color _background;
        private Color _foreground;
        private FontFamily _fontFamily;
        private readonly SettingsService _settingService;
        private Settings _settings;
        private SegmentViewModel _selectedSegment;

        public Color Background
        {
            get => _background;
            set => SetProperty(ref _background, value);
        }

        public Color Foreground
        {
            get => _foreground;
            set => SetProperty(ref _foreground, value);
        }

        public FontFamily FontFamily
        {
            get => _fontFamily;
            set
            {
                if (SetProperty(ref _fontFamily, value))
                {
                    foreach (var segmentViewModel in Segments)
                    {
                        segmentViewModel.Font = FontFamily;
                    }
                }
            }
        }

        public FontFamily SystemFontFamily { get; set; }

        public IReadOnlyList<Color> DefinedColors { get; }
        public IEnumerable<FontFamily> FontCollection => Fonts.SystemFontFamilies.OrderBy(x => x.ToString());

        public Settings Settings
        {
            get => _settings;

            set => SetProperty(ref _settings, value);
        }

        public SegmentViewModel SelectedSegment
        {
            get => _selectedSegment;
            set => SetProperty(ref _selectedSegment, value);
        }

        public ObservableCollection<SegmentViewModel> Segments { get; }

        public RelayCommand DonateCommand { get; }
        public RelayCommand DocsCommand { get; }
        public RelayCommand GithubCommand { get; }
        public RelayCommand ResetSettingsCommand { get; }
        public RelayCommand AddSegmentConfigurationCommand { get; }
        public RelayCommand RemoveSegmentConfigurationCommand { get; }

        public MainViewModel()
        {
            DefinedColors = typeof(Brushes).GetProperties().Select(x => ((SolidColorBrush)x.GetValue(null))!.Color).ToList();

            _settingService = new SettingsService();

            Settings = _settingService.Load();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += OnTick;
            timer.Start();

            Segments = new ObservableCollection<SegmentViewModel>();

            DonateCommand = new RelayCommand(() => ProcessService.Open(Links.Coffee));
            GithubCommand = new RelayCommand(() => ProcessService.Open(Links.Github));
            DocsCommand = new RelayCommand(() => ProcessService.Open(Links.DateTimeDocs));
            ResetSettingsCommand = new RelayCommand(ResetSettings);
            AddSegmentConfigurationCommand = new RelayCommand(AddConfigurationSegment);
            RemoveSegmentConfigurationCommand = new RelayCommand(RemoveConfigurationSegment);
        }

        private void AddConfigurationSegment()
        {
            var selectedIndex = Segments.IndexOf(_selectedSegment) + 1;

            Segments.Insert(selectedIndex, new SegmentViewModel(new SegmentConfiguration { Font = FontFamily.ToString() }));

            //Analytics.TrackEvent("segment-added");
        }

        private void RemoveConfigurationSegment()
        {
            if (_selectedSegment is { } segmentConfigurationViewModel)
            {
                _ = Segments.Remove(segmentConfigurationViewModel);
                SelectedSegment = Segments.FirstOrDefault();

                //Analytics.TrackEvent("segment-removed");
            }
        }

        public void Load()
        {
            Settings = _settingService.Load();

            foreach (var segmentConfiguration in Settings.Segments)
            {
                Segments.Add(new SegmentViewModel(segmentConfiguration));
            }

            SelectedSegment = Segments.FirstOrDefault();

            UpdateVisuals();
        }

        public void Save()
        {
            Settings.Visuals.Background = Background.ToString();
            Settings.Visuals.Foreground = Foreground.ToString();
            Settings.Visuals.FontFamily = FontFamily.ToString();

            Settings = _settings with { Segments = Segments.Select(x => x.GetConfiguration()).ToArray() };

            _settingService.Save(Settings);
        }

        public void ResetSettings()
        {
            Settings = _settingService.GetDefaults();

            UpdateVisuals();

            OnPropertyChanged();

            Segments.Clear();

            foreach (var segmentConfiguration in Settings.Segments)
            {
                Segments.Add(new SegmentViewModel(segmentConfiguration));
            }

            SelectedSegment = Segments.FirstOrDefault();

            //Analytics.TrackEvent("settings-reset");
        }

        private void UpdateVisuals()
        {
            FontFamily = FontCollection.FirstOrDefault(x => x.ToString() == Settings.Visuals.FontFamily) ?? SystemFontFamily;

            Background = (Color)(ColorConverter.ConvertFromString(Settings.Visuals.Background) ?? Colors.Black);
            Foreground = (Color)(ColorConverter.ConvertFromString(Settings.Visuals.Foreground) ?? Colors.White);
        }

        private void OnTick(object sender, EventArgs e) => UpdateSegments();

        private void UpdateSegments()
        {
            var now = DateTime.Now;

            foreach (var segment in Segments)
            {
                segment.Update(now);
            }
        }

        private void Donate() => _ = Process.Start(new ProcessStartInfo("https://www.buymeacoffee.com/arpad.barta") { UseShellExecute = true });
    }
}
