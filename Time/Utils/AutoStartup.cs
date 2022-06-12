using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Time.Utils
{
    public static class AutoStartup
    {
        private const string RUN_REGISTRY_KEY = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public static bool IsEnabled()
        {
            var processDescription = GetProcessDescription();
            var key = Registry.CurrentUser.OpenSubKey(RUN_REGISTRY_KEY, true);

            return key?.GetValue(processDescription.Name) is not null;
        }

        public static void Enable()
        {
            var processDescription = GetProcessDescription();

            var key = Registry.CurrentUser.OpenSubKey(RUN_REGISTRY_KEY, true);
            
            key?.SetValue(processDescription.Name, processDescription.Path);
        }

        public static void Disable()
        {
            var processDescription = GetProcessDescription();

            var key = Registry.CurrentUser.OpenSubKey(RUN_REGISTRY_KEY, true);

            key?.DeleteValue(processDescription.Name);
        }

        private static (string Path, string Name) GetProcessDescription()
        {
            var processFileName = System.Environment.ProcessPath;

            return (processFileName, Path.GetFileNameWithoutExtension(processFileName));
        }
    }
}
