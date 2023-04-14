using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Time.Services;
using Time.Translations;

namespace Time.ViewModels
{
    public record FormatDefinition(string Name, string Group, string Format);

    public record TimeZone(string Id, string DisplayName, string StandardName);
    public record SegmentConfiguration
    {
        public string Format { get; init; } = "t";
        public double Size { get; init; } = 12;
        public string Font { get; init; } = "Segoe UI";
        public string ZoneId { get; init; } = "local";
        public string Prefix { get; init; }
        public string Suffix { get; init; }
    }

    public class SegmentViewModel : ObservableObject
    {
        private const string CustomFormat = "custom-format";
        private const string LocalZone = "local";

        private static readonly FormatDefinition[] _formats;

        private string _content;

        private double _size;
        private FontFamily _font;
        private string _format;
        private FormatDefinition _selectedFormat;
        private string _prefix;
        private string _suffix;
        private TimeZone _selectedTimeZone;
        private TimeZoneInfo _selectedTimeZoneInfo;

        public string Content
        {
            get => _content;
            private set => SetProperty(ref _content, value);
        }

        public string Prefix
        {
            get => _prefix;
            set => SetProperty(ref _prefix, value);
        }

        public string Suffix
        {
            get => _suffix;
            set => SetProperty(ref _suffix, value);
        }

        public double Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public FontFamily Font
        {
            get => _font;
            set => SetProperty(ref _font, value);
        }

        public string Format
        {
            get => _format;
            set
            {
                if (SetProperty(ref _format, value))
                {
                    Update(DateTime.Now);
                }
            }
        }

        public ListCollectionView Formats { get; }

        public FormatDefinition SelectedFormat
        {
            get => _selectedFormat;
            set
            {
                if (SetProperty(ref _selectedFormat, value))
                {
                    OnPropertyChanged(nameof(IsCustom));

                    if (_selectedFormat?.Format != CustomFormat)
                    {
                        Format = _selectedFormat?.Format;
                    }
                }
            }
        }

        public bool IsCustom => _selectedFormat?.Format == CustomFormat;

        public IReadOnlyCollection<TimeZone> TimeZones { get; }

        public TimeZone SelectedTimeZone
        {
            get => _selectedTimeZone;
            set => SetProperty(_selectedTimeZone, value, OnTimeZoneChanged);
        }

        public RelayCommand OpenDocsCommand { get; }

        static SegmentViewModel()
        {
            _formats = new[]
            {
                new FormatDefinition(Resource.ShortTimeLabel, Resource.TimeLabel, "t"),
                new FormatDefinition(Resource.LongTimeLabel, Resource.TimeLabel, "T"),
                new FormatDefinition(Resource.ShortDayLabel, Resource.DayLabel, "ddd"),
                new FormatDefinition(Resource.LongDayLabel, Resource.DayLabel, "dddd"),
                new FormatDefinition(Resource.ShortDateLabel, Resource.DateLabel, "d"),
                new FormatDefinition(Resource.LongDateLabel, Resource.DateLabel, "D"),
                new FormatDefinition(Resource.CustomFormatLabel, Resource.CustomFormatLabel, CustomFormat),
            };
        }

        public SegmentViewModel(SegmentConfiguration configuration)
        {
            _size = configuration.Size;
            _format = configuration.Format;
            _font = new FontFamily(configuration.Font);
            _prefix = configuration.Prefix;
            _suffix = configuration.Suffix;

            Formats = new ListCollectionView(_formats);
            Formats?.GroupDescriptions?.Add(new PropertyGroupDescription(nameof(FormatDefinition.Group)));

            TimeZones = TimeZoneInfo.GetSystemTimeZones()
                .Select(x => new TimeZone(x.Id, x.DisplayName, x.StandardName))
                .Prepend(new TimeZone(LocalZone, "Local", "System defined time zone"))
                .ToArray();

            SelectedTimeZone = TimeZones.FirstOrDefault(x => x.Id == configuration.ZoneId);

            SelectedFormat = _formats.FirstOrDefault(x => x.Format == configuration.Format) ?? _formats.FirstOrDefault(x => x.Format == CustomFormat);

            OpenDocsCommand = new RelayCommand(() => ProcessService.Open(Links.DateTimeDocs));
        }

        

        public void Update(DateTime dateTime)
        {
            ArgumentNullException.ThrowIfNull(dateTime);

            try
            {
                Content = TimeZoneInfo.ConvertTime(dateTime, _selectedTimeZoneInfo).ToString(Format);
            }
            catch (Exception)
            {
                Content = "Invalid format.";
            }
        }

        public SegmentConfiguration GetConfiguration() => new()
        {
            Size = _size,
            Font = _font.ToString(),
            Format = _format,
            ZoneId = _selectedTimeZone.Id,
            Prefix = _prefix,
            Suffix = _suffix
        };

        private void OnTimeZoneChanged(TimeZone zone)
        {
            _selectedTimeZone = zone ?? TimeZones.First(x => x.Id == LocalZone);

            if (_selectedTimeZone.Id == LocalZone)
            {
                _selectedTimeZoneInfo = TimeZoneInfo.Local;
            }
            else
            {
                _selectedTimeZoneInfo = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id == _selectedTimeZone.Id) ?? TimeZoneInfo.Local;
            }

            Update(DateTime.Now);
        }
    }
}
