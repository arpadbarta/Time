using System.Windows;
using Time.ViewModels;

namespace Time
{
    public partial class App : Application
    {
        public App()
        {
            Exit += OnExit;
            SessionEnding += OnSessionEnding;
        }

        private void OnSessionEnding(object sender, SessionEndingCancelEventArgs e) => SaveWindowSettings();

        private void OnExit(object sender, ExitEventArgs e) => SaveWindowSettings();

        private void SaveWindowSettings()
        {
            foreach (var window in Windows)
            {
                if (window is MainWindow { DataContext: MainViewModel viewModel })
                {
                    viewModel.Save();
                }
            }
        }
    }
}
