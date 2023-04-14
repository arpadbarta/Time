using System.Diagnostics;
using Microsoft.AppCenter.Analytics;

namespace Time.Services
{
    public record UrlDefinition(string Name, string Path);

    public class ProcessService
    {
        public static void Open(UrlDefinition definition)
        {
            Analytics.TrackEvent($"opened-{definition.Name}");

            _ = Process.Start(new ProcessStartInfo(definition.Path) { UseShellExecute = true });
        }
    }
}
