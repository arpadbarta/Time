﻿using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Time.Translations;

namespace Time.ViewModels
{
    public record FormatDefinition(string Name, string Group, string Format);

    public record SegmentConfiguration
    {
        public string Format { get; init; } = "t";
        public double Size { get; init; } = 12;
        public string Font { get; init; } = "Segoe UI";
    }

    public class SegmentViewModel : ObservableObject
    {
        private const string CustomFormat = "custom-format";

        private string _content;

        private double _size;
        private FontFamily _font;
        private string _format;
        private FormatDefinition _selectedFormat;

        public string Content
        {
            get => _content;
            private set => SetProperty(ref _content, value);
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
                    Update(DateTimeOffset.Now);
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

        public SegmentViewModel(SegmentConfiguration configuration)
        {
            _size = configuration.Size;
            _format = configuration.Format;
            _font = new FontFamily(configuration.Font);

            var formats = new[]
            {
                new FormatDefinition(Resource.ShortTimeLabel, Resource.TimeLabel, "t"),
                new FormatDefinition(Resource.LongTimeLabel, Resource.TimeLabel, "T"),
                new FormatDefinition(Resource.ShortDayLabel, Resource.DayLabel, "ddd"),
                new FormatDefinition(Resource.LongDayLabel, Resource.DayLabel, "dddd"),
                new FormatDefinition(Resource.ShortDateLabel, Resource.DateLabel, "d"),
                new FormatDefinition(Resource.LongDateLabel, Resource.DateLabel, "D"),
                new FormatDefinition(Resource.CustomFormatLabel, Resource.CustomFormatLabel, CustomFormat),
            };

            Formats = new ListCollectionView(formats);
            Formats?.GroupDescriptions?.Add(new PropertyGroupDescription(nameof(FormatDefinition.Group)));

            SelectedFormat = formats.FirstOrDefault(x => x.Format == configuration.Format) ?? formats.FirstOrDefault(x => x.Format == CustomFormat);
        }

        public void Update(DateTimeOffset dateTimeOffset)
        {
            ArgumentNullException.ThrowIfNull(dateTimeOffset);

            try
            {
                Content = dateTimeOffset.ToString(Format);
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
            Format = _format
        };
    }
}