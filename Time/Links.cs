using Time.Services;

namespace Time
{
    public class Links
    {
        public static UrlDefinition Github { get; } = new(nameof(Github), "https://github.com/arpadbarta/Time");
        public static UrlDefinition Coffee { get; } = new(nameof(Coffee), "https://www.buymeacoffee.com/arpad.barta");
        public static UrlDefinition DateTimeDocs { get; } = new(nameof(DateTimeDocs), "https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers");
    }
}
