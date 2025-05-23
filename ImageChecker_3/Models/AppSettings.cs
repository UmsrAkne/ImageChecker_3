using System.IO;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class AppSettings : BindableBase
    {
        // ReSharper disable once ArrangeModifiersOrder
        public const string SettingFileName = "appSetting.json";

        private string imageTagText = @"<image a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" targetLayerIndex=""$targetLayerIndex"" />";
        private string drawTagText = @"<draw b=""$b"" c=""$c"" d=""$d"" targetLayerIndex=""$targetLayerIndex"" />";
        private string animationImageTagText = @"<anime name=""image"" a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" targetLayerIndex=""$targetLayerIndex"" />";
        private string animationDrawTagText = @"<anime name=""draw"" b=""$b"" c=""$c"" d=""$d"" targetLayerIndex=""$targetLayerIndex"" />";
        private string slideTagText = @"<anime name=""slide"" duration=""$duration"" distance=""$distance"" degree=""$degree"" repeatCount=""0"" targetLayerIndex=""$targetLayerIndex"" />";

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

        public string SlideTagText { get => slideTagText; set => SetProperty(ref slideTagText, value); }

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

        /// <summary>
        /// 引数に入力された別のオブジェクトと、このオブジェクトが等価であるかを判定します。
        /// </summary>
        /// <param name="other">別のオブジェクトを入力します。</param>
        /// <returns>入力されたオブジェクトとこのオブジェクトが等価であるか。</returns>
        public bool IsEqualTo(AppSettings other)
        {
            var a = ImageTagText == other.ImageTagText;
            var b = DrawTagText == other.DrawTagText;
            var c = AnimationImageTagText == other.AnimationImageTagText;
            var d = AnimationDrawTagText == other.AnimationDrawTagText;
            var e = SlideTagText == other.SlideTagText;

            return a && b && c && d && e;
        }
    }
}