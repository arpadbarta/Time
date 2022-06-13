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

        public double Width
        {
            get => _width;
            set => Set(ref _width, value);
        }
        
        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
        }
        
        public double Top
        {
            get => _top;
            set => Set(ref _top, value);
        }
        
        public double Left
        {
            get => _left;
            set => Set(ref _left, value);
        }

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => Set(ref _alwaysOnTop, value);
        }

        public bool AllowResize
        {
            get => _allowResize;
            set => Set(ref _allowResize, value);
        }

        public bool ShowInTaskbar
        {
            get => _showInTaskbar;
            set => Set(ref _showInTaskbar, value);
        }

        public double CornerRadius
        {
            get => _cornerRadius;
            set => Set(ref _cornerRadius, value);
        }
    }
}
