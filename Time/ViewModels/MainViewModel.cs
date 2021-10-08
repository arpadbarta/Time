﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace Time.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool _isShortTime;
        private bool _isShortDate;
        private string _time;
        private string _date;
        private double _opacity;
        private double _cornerRadius;
        private bool _showInTaskbar;
        private bool _allowResize;
        private Color _background;
        private Color _foreground;

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

        public bool IsShortTime
        {
            get => _isShortTime;
            set => Set(ref _isShortTime, value, UpdateDateTime);
        }

        public bool IsShortDate
        {
            get => _isShortDate;
            set => Set(ref _isShortDate, value, UpdateDateTime);
        }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value);
        }

        public double CornerRadius
        {
            get => _cornerRadius;
            set => Set(ref _cornerRadius, value);
        }

        public bool ShowInTaskbar
        {
            get => _showInTaskbar;
            set => Set(ref _showInTaskbar, value);
        }

        public bool AllowResize
        {
            get => _allowResize;
            set => Set(ref _allowResize, value);
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

        public IReadOnlyList<Color> Colors { get; }

        public MainViewModel()
        {
            Colors = typeof(Brushes).GetProperties().Select(x => ((SolidColorBrush)x.GetValue(null)).Color).ToList();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object sender, EventArgs e) => UpdateDateTime();

        private void UpdateDateTime()
        {
            var now = DateTime.Now;

            Time = _isShortTime ? now.ToShortTimeString() : now.ToLongTimeString();
            Date = _isShortDate ? now.ToShortDateString() : now.ToLongDateString();
        }
    }
}