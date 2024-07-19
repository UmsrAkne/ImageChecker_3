using System;
using System.Globalization;
using System.Windows;
using ImageChecker_3.Models.Images;
using Prism.Commands;

namespace ImageChecker_3.Models
{
    public class TagGenerator
    {
        public event EventHandler TagGenerated;

        public DelegateCommand<PreviewContainer> CopyImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(ImageTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(DrawTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(AnimationImageTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            Clipboard.SetText(GetTag(AnimationDrawTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        private string ImageTagText { get; set; } = string.Empty;

        private string DrawTagText { get; set; } = string.Empty;

        private string AnimationImageTagText { get; set; } = string.Empty;

        private string AnimationDrawTagText { get; set; } = string.Empty;

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

        public void SetSettings(AppSettings appSettings)
        {
            ImageTagText = appSettings.ImageTagText;
            DrawTagText = appSettings.DrawTagText;
            AnimationImageTagText = appSettings.AnimationImageTagText;
            AnimationDrawTagText = appSettings.AnimationDrawTagText;
        }
    }
}