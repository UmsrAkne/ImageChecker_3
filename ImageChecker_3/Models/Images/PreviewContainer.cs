using System.Collections.ObjectModel;
using System.Linq;
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
            set
            {
                if (SetProperty(ref screenRect, value))
                {
                    RaisePropertyChanged(nameof(RelativePosition));
                    RaisePropertyChanged(nameof(X));
                    RaisePropertyChanged(nameof(Y));
                }
            }
        }

        /// <summary>
        /// このコンテナの中に格納されている画像のうち、一番大きな画像のサイズを取得します。
        /// </summary>
        public Size MaxImageSize { get; private set; }

        /// <summary>
        /// 実際の画面のサイズに対する、プレビュー領域のサイズ比です。
        /// 例えば、このプロパティの値が 0.5 ならば、画面サイズに対してプレビュー領域が半分の大きさとなります。
        /// </summary>
        public double PreviewScale { get => previewScale; set => SetProperty(ref previewScale, value); }

        /// <summary>
        /// 実際のプレイ時に表示される画像のスケールです。
        /// </summary>
        public double Scale
        {
            get => scale;
            set
            {
                SetProperty(ref scale, value);
                RaisePropertyChanged(nameof(RelativePosition));
            }
        }

        /// <summary>
        /// スクリーン中央を基準とした、表示中の画像の相対位置を取得します。
        /// </summary>
        public Point RelativePosition
        {
            get
            {
                var sRect = ScreenRect with { X = 0, Y = 0, };
                var imgRect = new Rect(ScreenRect.X, ScreenRect.Y, MaxImageSize.Width * Scale, MaxImageSize.Height * Scale);
                var imgCenter = new Point((imgRect.Width / 2) + imgRect.X, (imgRect.Height / 2) + imgRect.Y);
                var sRectCenter = new Point(sRect.Width / 2, sRect.Height / 2);
                return new Point(imgCenter.X - sRectCenter.X, imgCenter.Y - sRectCenter.Y);
            }
        }

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

            MaxImageSize = new Size(
                ImageWrappers.Max(w => w.ImageFileInfo.Width),
                ImageWrappers.Max(w => w.ImageFileInfo.Height));

            RaisePropertyChanged(nameof(MaxImageSize));
        }
    }
}