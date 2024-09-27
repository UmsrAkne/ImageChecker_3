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
            Title = "ImageChecker3";

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
            Version = "20240927" + "a";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}