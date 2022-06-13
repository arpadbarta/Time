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
        set => Set(ref _isShortTime, value);
    }
        
    public bool IsShortDate
    {
        get => _isShortDate;
        set => Set(ref _isShortDate, value);
    }
    
    public bool ShowDate
    {
        get => _showDate;
        set => Set(ref _showDate, value);
    }

    public bool ShowDay
    {
        get => _showDay;
        set => Set(ref _showDay, value);
    }
}