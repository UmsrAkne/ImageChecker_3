using System.Globalization;
using System.Windows;
using ImageChecker_3.Models.Images;
using Prism.Commands;

namespace ImageChecker_3.Models
{
    public class TagGenerator
    {
        public DelegateCommand<PreviewContainer> CopyImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(ImageTagText, param));
        });

        public DelegateCommand<PreviewContainer> CopyDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(DrawTagText, param));
        });

        public DelegateCommand<PreviewContainer> CopyAnimationImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(AnimationImageTagText, param));
        });

        public DelegateCommand<PreviewContainer> CopyAnimationDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(AnimationDrawTagText, param));
        });

        private string ImageTagText { get; set; } = @"<image a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";

        private string DrawTagText { get; set; } = @"<draw b=""$b"" c=""$c"" d=""$d"" />";

        private string AnimationImageTagText { get; set; } = @"<anime name=""image"" a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";

        private string AnimationDrawTagText { get; set; } = @"<anime name=""draw"" b=""$b"" c=""$c"" d=""$d"" />";

        public static string GetTag(string baseText, PreviewContainer previewContainer)
        {
            var relPosition = previewContainer.RelativePosition;
            var ws = previewContainer.GetImageFileNames();
            return baseText
                .Replace("$a", ws[0])
                .Replace("$b", ws[1])
                .Replace("$c", ws[2])
                .Replace("$d", ws[3])
                .Replace("$scale", previewContainer.Scale.ToString("0.0", CultureInfo.InvariantCulture))
                .Replace("$x", ((int)relPosition.X).ToString(CultureInfo.CurrentCulture))
                .Replace("$y", ((int)relPosition.Y).ToString(CultureInfo.CurrentCulture));
        }
    }
}