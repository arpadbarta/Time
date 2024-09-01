using CommunityToolkit.Mvvm.ComponentModel;

namespace Time.ViewModels
{
    public class WindowProperties : ObservableObject
    {
        private double _width;
        private double _left;
        private double _top;
        private double _height;
        private bool _alwaysOnTop;
        private bool _allowResize;
        private bool _showInTaskbar;
        private double _cornerRadius;
        private bool _allowMove;
        private bool _showButtons;

        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => SetProperty(ref _alwaysOnTop, value);
        }

        public bool AllowResize
        {
            get => _allowResize;
            set => SetProperty(ref _allowResize, value);
        }

        public bool AllowMove
        {
            get => _allowMove;
            set => SetProperty(ref _allowMove, value);
        }

        public bool ShowInTaskbar
        {
            get => _showInTaskbar;
            set => SetProperty(ref _showInTaskbar, value);
        }

        public double CornerRadius
        {
            get => _cornerRadius;
            set => SetProperty(ref _cornerRadius, value);
        }

        public bool ShowButtons
        {
            get => _showButtons;
            set => SetProperty(ref _showButtons, value);
        }

        public WindowProperties()
        {
            Top = 150;
            Left = 150;
            Width = 360;
            Height = 250;
            AlwaysOnTop = true;
            AllowResize = true;
            ShowInTaskbar = true;
            AllowMove = true;
            ShowButtons = true;
        }
    }
}
