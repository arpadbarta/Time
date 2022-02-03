using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Time.Properties;
using Time.ViewModels;

namespace Time
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            MouseDown += OnMouseDown;

            LoadSettings();
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();

            base.OnClosed(e);
        }

        private void SaveSettings()
        {
            var context = (MainViewModel)DataContext;

            Settings.Default.Opacity = context.Opacity;
            Settings.Default.CornerRadius = context.CornerRadius;
            Settings.Default.Background = System.Drawing.Color
                .FromArgb(255, context.Background.R, context.Background.G, context.Background.B).ToArgb();
            Settings.Default.Foreground = System.Drawing.Color
                .FromArgb(255, context.Foreground.R, context.Foreground.G, context.Foreground.B).ToArgb();
            Settings.Default.FontFamily = context.FontFamily?.ToString();
            Settings.Default.UsOpacityForFont = context.ApplyOpacityToFont;

            Settings.Default.AllowResize = context.AllowResize;
            Settings.Default.ShowInTaskbar = context.ShowInTaskbar;
            Settings.Default.AlwaysOnTop = context.AlwaysOnTop;

            Settings.Default.IsShortTime = context.IsShortTime;
            Settings.Default.IsShortDate = context.IsShortDate;
            Settings.Default.IsDateVisible = context.IsDateVisible;

            Settings.Default.Left = Left;
            Settings.Default.Top = Top;
            Settings.Default.Width = Width;
            Settings.Default.Height = Height;

            Settings.Default.Save();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                OpenSettings();
            }
        }

        private void LoadSettings()
        {
            var context = (MainViewModel)DataContext;

            context.Opacity = Settings.Default.Opacity;
            context.CornerRadius = Settings.Default.CornerRadius;

            context.AllowResize = Settings.Default.AllowResize;
            context.ShowInTaskbar = Settings.Default.ShowInTaskbar;

            context.IsShortTime = Settings.Default.IsShortTime;
            context.IsShortDate = Settings.Default.IsShortDate;
            context.IsDateVisible = Settings.Default.IsDateVisible;

            context.AlwaysOnTop = Settings.Default.AlwaysOnTop;

            var foreground = System.Drawing.Color.FromArgb(Settings.Default.Foreground);
            context.Foreground = Color.FromRgb(foreground.R, foreground.G, foreground.B);

            var background = System.Drawing.Color.FromArgb(Settings.Default.Background);
            context.Background = Color.FromRgb(background.R, background.G, background.B);

            context.FontFamily = context.FontCollection.FirstOrDefault(x => x.ToString() == (Settings.Default.FontFamily ?? FontFamily.ToString()));
            context.ApplyOpacityToFont = Settings.Default.UsOpacityForFont;

            Left = Settings.Default.Left;
            Top = Settings.Default.Top;
            Width = Settings.Default.Width;
            Height = Settings.Default.Height;
        }

        private void OnSettingsMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            OpenSettings();
        }

        private void OpenSettings()
        {
            var settingsWindow = Application.Current.Windows.OfType<SettingsWindow>().FirstOrDefault();

            if (settingsWindow is not null)
            {
                if (settingsWindow.WindowState == WindowState.Minimized)
                {
                    settingsWindow.WindowState = WindowState.Normal;
                }

                _ = settingsWindow.Activate();
            }
            else
            {
                settingsWindow = new SettingsWindow
                {
                    DataContext = DataContext
                };

                settingsWindow.Show();
            }
        }

        private void OnCloseMouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveSettings();
            Application.Current.Shutdown();
        }

        private void OnDonateMouseDown(object sender, MouseButtonEventArgs e)
        {
            _ = Process.Start(new ProcessStartInfo("https://www.buymeacoffee.com/arpad.barta") { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
