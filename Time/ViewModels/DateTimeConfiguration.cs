using CommunityToolkit.Mvvm.ComponentModel;

namespace Time.ViewModels;

public class DateTimeConfiguration : ObservableObject
{
    private bool _isShortTime;
    private bool _isShortDate;
    private bool _showDay;
    private bool _showDate;

    public bool IsShortTime
    {
        get => _isShortTime;
        set => SetProperty(ref _isShortTime, value);
    }
        
    public bool IsShortDate
    {
        get => _isShortDate;
        set => SetProperty(ref _isShortDate, value);
    }
    
    public bool ShowDate
    {
        get => _showDate;
        set => SetProperty(ref _showDate, value);
    }

    public bool ShowDay
    {
        get => _showDay;
        set => SetProperty(ref _showDay, value);
    }
}