using System.Collections.ObjectModel;
using System.Windows;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer : BindableBase
    {
        private double previewScale = 0.5;
        private Rect screenRect = new Rect(0, 0, 1280, 720);
        private double scale = 1.0;

        public ObservableCollection<ImageWrapper> ImageWrappers { get; set; } = new ();

        /// <summary>
        /// 実際のプレイ時の画面のサイズを表します。
        /// </summary>
        public Rect ScreenRect
        {
            get => screenRect;
            set => SetProperty(ref screenRect, value);
        }

        /// <summary>
        /// 実際の画面のサイズに対する、プレビュー領域のサイズ比です。
        /// 例えば、このプロパティの値が 0.5 ならば、画面サイズに対してプレビュー領域が半分の大きさとなります。
        /// </summary>
        public double PreviewScale { get => previewScale; set => SetProperty(ref previewScale, value); }

        /// <summary>
        /// 実際のプレイ時に表示される画像のスケールです。
        /// </summary>
        public double Scale { get => scale; set => SetProperty(ref scale, value); }

        public double X
        {
            get => ScreenRect.X;
            set => ScreenRect = ScreenRect with { X = value, };
        }

        public double Y
        {
            get => ScreenRect.Y;
            set => ScreenRect = ScreenRect with { Y = value, };
        }

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });
        }
    }
}