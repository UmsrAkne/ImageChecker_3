using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using ImageChecker_3.Models.Images;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class TagGenerator : BindableBase
    {
        public event EventHandler TagGenerated;

        public ObservableCollection<string> ClipboardHistory { get; set; } = new ();

        public DelegateCommand<PreviewContainer> CopyImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.Image;
            Clipboard.SetText(GetTag(ImageTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.Draw;
            Clipboard.SetText(GetTag(DrawTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.AnimationImage;
            Clipboard.SetText(GetTag(AnimationImageTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.AnimationDraw;
            Clipboard.SetText(GetTag(AnimationDrawTagText, param));
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<SlideController> CopySlideTagCommand => new DelegateCommand<SlideController>((param) =>
        {
            if (param == null)
            {
                return;
            }

            var text = GetTag(SlideTagText, param);
            Clipboard.SetText(GetTag(SlideTagText, param));
            ClipboardHistory.Add(text);
        });

        public DelegateCommand<string> CopySlideTagFromHistoryCommand => new ((param) =>
        {
            if (string.IsNullOrEmpty(param))
            {
                return;
            }

            Clipboard.SetText(param);
        });

        private string ImageTagText { get; set; } = string.Empty;

        private string DrawTagText { get; set; } = string.Empty;

        private string AnimationImageTagText { get; set; } = string.Empty;

        private string AnimationDrawTagText { get; set; } = string.Empty;

        private string SlideTagText { get; set; } = string.Empty;

        public static string GetTag(string baseText, PreviewContainer previewContainer)
        {
            var relPosition = previewContainer.RelativePosition;
            var ws = previewContainer.GetImageFileNames();
            var tag = baseText
                .Replace("$a", ws[0])
                .Replace("$b", ws[1])
                .Replace("$c", ws[2])
                .Replace("$d", ws[3])
                .Replace("$scale", previewContainer.Scale.ToString("0.0", CultureInfo.InvariantCulture))
                .Replace("$x", ((int)relPosition.X).ToString(CultureInfo.CurrentCulture))
                .Replace("$y", ((int)relPosition.Y).ToString(CultureInfo.CurrentCulture));

            var id = GetId(tag);
            tag = tag.Replace("/>", $"id=\"{id}\" />");
            return tag;
        }

        public static string GetTag(string baseText, SlideController slideController)
        {
            var tag = baseText
                .Replace("$distance", slideController.Distance.ToString("0", CultureInfo.InvariantCulture))
                .Replace("$degree", slideController.Degree.ToString("0", CultureInfo.InvariantCulture))
                .Replace("$duration", slideController.Duration.ToString("0", CultureInfo.InvariantCulture));

            var id = GetId(tag);
            tag = tag.Replace("/>", $"id=\"{id}\" />");

            return tag;
        }

        public void SetSettings(AppSettings appSettings)
        {
            ImageTagText = appSettings.ImageTagText;
            DrawTagText = appSettings.DrawTagText;
            AnimationImageTagText = appSettings.AnimationImageTagText;
            AnimationDrawTagText = appSettings.AnimationDrawTagText;
            SlideTagText = appSettings.SlideTagText;
        }

        private static string GetId(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var base64Hash = Convert.ToBase64String(hashBytes);

            var alphabetId = base64Hash.Select(c =>
            {
                if (c is >= '0' and <= '9')
                {
                    return (char)('g' + int.Parse(c.ToString()));
                }

                return c;
            }).Select(c => c.ToString().ToLower().First())
            .Take(8).ToArray();

            return new string(alphabetId.ToArray());
        }
    }
}