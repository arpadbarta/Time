using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Time.ViewModels;

namespace Time
{
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = new MainViewModel
            {
                SystemFontFamily = FontFamily
            };

            _viewModel.Load();

            DataContext = _viewModel;

            MouseDown += OnMouseDown;
            Closing += OnClosing;
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

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e) => _viewModel.Save();

        private void OnCloseMouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Save();
            Application.Current.Shutdown();
        }

        private void OnDonateMouseDown(object sender, MouseButtonEventArgs e)
        {
            _ = Process.Start(new ProcessStartInfo("https://www.buymeacoffee.com/arpad.barta") { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
