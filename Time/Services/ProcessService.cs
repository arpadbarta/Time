using System.Diagnostics;

namespace Time.Services
{
    public record UrlDefinition(string Name, string Path);

    public class ProcessService
    {
        public static void Open(UrlDefinition definition)
        {
            _ = Process.Start(new ProcessStartInfo(definition.Path) { UseShellExecute = true });
        }
    }
}
