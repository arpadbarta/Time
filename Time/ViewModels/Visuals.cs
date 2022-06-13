namespace Time.ViewModels;

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
        set => Set(ref _foreground, value);
    }

    public string Background
    {
        get => _background;
        set => Set(ref _background, value);
    }

    public string FontFamily
    {
        get => _fontFamily;
        set => Set(ref _fontFamily, value);
    }

    public double Opacity
    {
        get => _opacity;
        set => Set(ref _opacity, value, () => RaisePropertyChanged(nameof(FontOpacity)));
    }

    public double FontOpacity => _applyOpacityToFont ? _opacity : 1;

    public bool ApplyOpacityToFont
    {
        get => _applyOpacityToFont;
        set => Set(ref _applyOpacityToFont, value, () => RaisePropertyChanged(nameof(FontOpacity)));
    }
}