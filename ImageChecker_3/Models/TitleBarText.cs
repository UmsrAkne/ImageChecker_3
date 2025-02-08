using System.Diagnostics;
using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class TitleBarText : BindableBase
    {
        private string title;
        private string version = string.Empty;

        public TitleBarText()
        {
            Title = "AppLauncher";

            SetVersion();
            AddDebugMark();
        }

        public string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title
                : title + " version : " + Version;
            private set => SetProperty(ref title, value);
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            const int major = 1;
            const int minor = 2;
            const int patch = 1;
            const string date = "20250208";
            const string suffixId = "a";

            Version = $"{major}.{minor}.{patch} ({date}{suffixId})";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}