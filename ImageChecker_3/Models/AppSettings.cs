using System.IO;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class AppSettings : BindableBase
    {
        public static readonly string SettingFileName = "appSetting.json";

        private string imageTagText = @"<image a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";
        private string drawTagText = @"<draw b=""$b"" c=""$c"" d=""$d"" />";
        private string animationImageTagText = @"<anime name=""image"" a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";
        private string animationDrawTagText = @"<anime name=""draw"" b=""$b"" c=""$c"" d=""$d"" />";

        public string ImageTagText { get => imageTagText; set => SetProperty(ref imageTagText, value); }

        public string DrawTagText { get => drawTagText; set => SetProperty(ref drawTagText, value); }

        public string AnimationImageTagText
        {
            get => animationImageTagText;
            set => SetProperty(ref animationImageTagText, value);
        }

        public string AnimationDrawTagText
        {
            get => animationDrawTagText;
            set => SetProperty(ref animationDrawTagText, value);
        }

        public static AppSettings LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new AppSettings();
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<AppSettings>(json);
        }

        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}