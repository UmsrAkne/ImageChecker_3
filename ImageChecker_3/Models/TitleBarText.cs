using System.Diagnostics;
using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class TitleBarText : BindableBase
    {
        private string title;
        private string version = string.Empty;
        private string currentDirectoryPath;

        public TitleBarText()
        {
            Title = "AppLauncher";

            SetVersion();
            AddDebugMark();
        }

        public string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title + $" {CurrentDirectoryPath}"
                : title + " version : " + Version + $" {CurrentDirectoryPath}";
            private set => SetProperty(ref title, value);
        }

        public string CurrentDirectoryPath
        {
            get => currentDirectoryPath;
            set
            {
                SetProperty(ref currentDirectoryPath, value);
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            const int major = 1;
            const int minor = 2;
            const int patch = 2;
            const string date = "20250209";
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