using CommunityToolkit.Mvvm.ComponentModel;

namespace Time.ViewModels
{
    public class Visuals : ObservableObject
    {
        private string _foreground;
        private string _background;
        private string _fontFamily;
        private double _opacity;
        private bool _applyOpacityToFont;

        public string Foreground
        {
            get => _foreground;
            set => SetProperty(ref _foreground, value);
        }

        public string Background
        {
            get => _background;
            set => SetProperty(ref _background, value);
        }

        public string FontFamily
        {
            get => _fontFamily;
            set => SetProperty(ref _fontFamily, value);
        }

        public double Opacity
        {
            get => _opacity;
            set
            {
                if (SetProperty(ref _opacity, value))
                {
                    OnPropertyChanged(nameof(FontOpacity));
                }
            }
        }

        public double FontOpacity => _applyOpacityToFont ? _opacity : 1;

        public bool ApplyOpacityToFont
        {
            get => _applyOpacityToFont;
            set
            {
                if (SetProperty(ref _applyOpacityToFont, value))
                {
                    OnPropertyChanged(nameof(FontOpacity));
                }
            }
        }
    }
}