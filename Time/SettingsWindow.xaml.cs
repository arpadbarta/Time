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

        private void ResetSettings(object sender, MouseButtonEventArgs e) => (DataContext as MainViewModel)?.ResetSettings();
        private void OnCloseMouseDown(object sender, MouseButtonEventArgs e) => Close();
    }
}
