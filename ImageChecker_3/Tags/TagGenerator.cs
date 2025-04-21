using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using ImageChecker_3.Images;
using ImageChecker_3.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.Tags
{
    public class TagGenerator : BindableBase
    {
        public event EventHandler TagGenerated;

        public ObservableCollection<string> ClipboardHistory { get; set; } = new ();

        public ObservableCollection<SlideTag> SlideTags { get; } = new ();

        public DelegateCommand<PreviewContainer> CopyImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.Image;
            var tag = GetTag(ImageTagText, param);
            Clipboard.SetText(tag);
            param.TagId = ExtractId(tag);
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.Draw;
            var tag = GetTag(DrawTagText, param);
            Clipboard.SetText(tag);
            param.TagId = ExtractId(tag);
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationImageTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.AnimationImage;
            var tag = GetTag(AnimationImageTagText, param);
            Clipboard.SetText(tag);
            param.TagId = ExtractId(tag);
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<PreviewContainer> CopyAnimationDrawTagCommand => new ((param) =>
        {
            if (param == null)
            {
                return;
            }

            param.TagType = TagType.AnimationDraw;
            var tag = GetTag(AnimationDrawTagText, param);
            Clipboard.SetText(tag);
            param.TagId = ExtractId(tag);
            TagGenerated?.Invoke(this, EventArgs.Empty);
        });

        public DelegateCommand<SlideController> CopySlideTagCommand => new DelegateCommand<SlideController>((param) =>
        {
            if (param == null)
            {
                return;
            }

            var text = GetTag(SlideTagText, param, GetLayerIndex(param.PreviewContainer.GetImageFileNames()));
            Clipboard.SetText(text);

            var slideTag = TagParser.LoadSlideTag(text);
            SlideTags.Add(slideTag);

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

        /// <summary>
        /// 入力文字列から一意性の高い8桁のアルファベットのみで構成されたIDを生成します。
        /// </summary>
        /// <param name="input">ID生成の元となる入力文字列。</param>
        /// <returns>生成された8桁のアルファベットで構成されるID。</returns>
        /// <remarks>
        /// このメソッドでは、入力文字列をSHA-256でハッシュ化し、そのハッシュ値をBase64エンコードします。
        /// Base64エンコードで生成される文字列には「+」や「/」などの記号が含まれる可能性があるため、
        /// それらをそれぞれ「x」および「y」に置き換えます。
        /// また、数値が含まれる場合には「g」以降のアルファベットに変換されます。
        /// 変換後の文字列の先頭8文字を小文字化してIDとして返します。
        /// このIDは完全な一意性を保証するものではありませんが、軽量な識別子として利用できます。
        /// </remarks>
        public static string GetId(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var base64Hash = Convert.ToBase64String(hashBytes);

            // base64 には +, / が含まれるため、それをアルファベットに置き換えておく。
            base64Hash = base64Hash.Replace('+', 'x').Replace('/', 'y');

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

        /// <summary>
        /// タグを表すテキストと PreviewContainer を受け取り、入力したテキスト内の情報を PreviewContainer の値で置き換え、ユニークな ID を付与したタグを取得します。
        /// </summary>
        /// <param name="baseText">一つのタグを表すテキストを入力します。</param>
        /// <param name="previewContainer">置き換える値のソースを入力します。</param>
        /// <param name="includeId">出力にタグを含めるかどうか。入力しない場合は ID を付与して出力します。</param>
        /// <returns>previewContainer の情報を使って生成したタグ。</returns>
        public static string GetTag(string baseText, PreviewContainer previewContainer, bool includeId = true)
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
                .Replace("$y", ((int)relPosition.Y).ToString(CultureInfo.CurrentCulture))
                .Replace("$targetLayerIndex", GetLayerIndex(ws).ToString());

            if (!includeId)
            {
                return tag;
            }

            var id = GetId(tag);
            tag = tag.Replace("/>", $"id=\"{id}\" />");

            return tag;
        }

        public static int GetLayerIndex(List<string> imageFileNames)
        {
            if (imageFileNames == null || imageFileNames.All(string.IsNullOrWhiteSpace))
            {
                return 0;
            }

            foreach (var imageFileName in imageFileNames)
            {
                if (int.TryParse(imageFileName.AsSpan(1, 1), out var n))
                {
                    return n;
                }
            }

            return 0;
        }

        public void SetSettings(AppSettings appSettings)
        {
            ImageTagText = appSettings.ImageTagText;
            DrawTagText = appSettings.DrawTagText;
            AnimationImageTagText = appSettings.AnimationImageTagText;
            AnimationDrawTagText = appSettings.AnimationDrawTagText;
            SlideTagText = appSettings.SlideTagText;
        }

        private static string ExtractId(string input)
        {
            var match = Regex.Match(input, @"id=""(.*?)""");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static string GetTag(string baseText, SlideController slideController, int targetLayerIndex = 0)
        {
            var tag = baseText
                .Replace("$distance", slideController.Distance.ToString("0", CultureInfo.InvariantCulture))
                .Replace("$degree", slideController.Degree.ToString("0", CultureInfo.InvariantCulture))
                .Replace("$duration", slideController.Duration.ToString("0", CultureInfo.InvariantCulture))
                .Replace("$targetLayerIndex", targetLayerIndex.ToString());

            var id = GetId(tag);
            tag = tag.Replace("/>", $"id=\"{id}\" />");

            return tag;
        }
    }
}