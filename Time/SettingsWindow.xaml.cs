using Microsoft.AppCenter.Analytics;
using System.Windows;
using System.Windows.Input;
using Time.ViewModels;

namespace Time
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private void OnCloseMouseDown(object sender, RoutedEventArgs e)
        {
            Close();
            Analytics.TrackEvent("settings-closed");
        }

        private void ResetSettings(object sender, RoutedEventArgs e) => (DataContext as MainViewModel)?.ResetSettings();
    }
}
