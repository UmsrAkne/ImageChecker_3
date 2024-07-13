using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer : BindableBase
    {
        private double previewScale = 0.5;
        private BindableRect screenRect = new (0, 0, 1280, 720);
        private double scale = 1.0;

        public ObservableCollection<ImageWrapper> ImageWrappers { get; set; } = new ();

        /// <summary>
        /// 実際のプレイ時の画面のサイズを表します。
        /// </summary>
        public BindableRect ScreenRect
        {
            get => screenRect;
            set => SetProperty(ref screenRect, value);
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
                var sRect = new Rect(0, 0, ScreenRect.Width, ScreenRect.Height);
                var imgRect = new Rect(ScreenRect.X, ScreenRect.Y, MaxImageSize.Width * Scale, MaxImageSize.Height * Scale);
                var imgCenter = new Point((imgRect.Width / 2) + imgRect.X, (imgRect.Height / 2) + imgRect.Y);
                var sRectCenter = new Point(sRect.Width / 2, sRect.Height / 2);
                return new Point(imgCenter.X - sRectCenter.X, imgCenter.Y - sRectCenter.Y);
            }

            set
            {
                // 任意の拡大率の画像が画面中央にセットされる座標を算出。
                var sRect = new Rect(0, 0, ScreenRect.Width, ScreenRect.Height);
                var imgX = (((MaxImageSize.Width * Scale) - sRect.Width) / 2) * -1;
                var imgY = (((MaxImageSize.Height * Scale) - sRect.Height) / 2) * -1;

                // 座標をセット。imgX,Y の座標が、相対座標 (0,0) であるので、それにそのまま value を足せば良い。
                ScreenRect.X = imgX + value.X;
                ScreenRect.Y = imgY + value.Y;
            }
        }

        public double X
        {
            get => ScreenRect.X;
            set
            {
                ScreenRect.X = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RelativePosition));
            }
        }

        public double Y
        {
            get => ScreenRect.Y;
            set
            {
                ScreenRect.Y = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RelativePosition));
            }
        }

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });

            var notNulls = ImageWrappers.Where(w => w != null);

            if (notNulls.Any())
            {
                MaxImageSize = new Size(
                    ImageWrappers.Max(w => w.ImageFileInfo.Width),
                    ImageWrappers.Max(w => w.ImageFileInfo.Height));
            }

            RaisePropertyChanged(nameof(MaxImageSize));
        }
    }
}