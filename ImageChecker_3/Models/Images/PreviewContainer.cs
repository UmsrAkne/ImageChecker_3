using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer : BindableBase
    {
        private double previewScale = 0.4;
        private BindableRect screenRect = new (0, 0, 0, 720);
        private double scale = 1.0;
        private bool isSelected;
        private List<ImageWrapper> originalImageWrappers;
        private string tagId = string.Empty;

        public ObservableCollection<ImageWrapper> ImageWrappers { get; private init; } = new () { null, null, null, null, };

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
                return new Point(imgCenter.X - sRectCenter.X, -(imgCenter.Y - sRectCenter.Y));
            }

            set
            {
                // 任意の拡大率の画像が画面中央にセットされる座標を算出。
                var sRect = new Rect(0, 0, ScreenRect.Width, ScreenRect.Height);
                var imgX = (((MaxImageSize.Width * Scale) - sRect.Width) / 2) * -1;
                var imgY = (((MaxImageSize.Height * Scale) - sRect.Height) / 2) * -1;

                // 座標をセット。imgX,Y の座標が、相対座標 (0,0) であるので、それにそのまま value を足せば良い。
                X = imgX + value.X;
                Y = imgY + value.Y;
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

        /// <summary>
        /// このプレビューコンテナから、タグを生成した際に、そのタグのタイプが入力されます。
        /// デフォルトでは TagType.NoType(0) が割り当てられています。
        /// </summary>
        public TagType TagType { get; set; }

        /// <summary>
        /// このプレビューコンテナから、タグを生成した際に生成された TagId です。
        /// </summary>
        public string TagId { get => tagId; set => SetProperty(ref tagId, value); }

        /// <summary>
        /// プレビューコンテナの履歴エリアで、選択中のアイテムを取得するための使用します。
        /// </summary>
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        public DelegateCommand<Position?> SetPositionCommand => new DelegateCommand<Position?>((param) =>
        {
            switch (param)
            {
                case Position.TopRight:
                    X = ((MaxImageSize.Width * Scale) - ScreenRect.Width) * -1;
                    Y = 0;
                    break;
                case Position.Left:
                    X = 0;
                    break;
                case Position.BottomLeft:
                    X = 0;
                    Y = ((MaxImageSize.Height * Scale) - ScreenRect.Height) * -1;
                    break;
                case Position.Bottom:
                    Y = ((MaxImageSize.Height * Scale) - ScreenRect.Height) * -1;
                    break;
                case Position.BottomRight:
                    X = ((MaxImageSize.Width * Scale) - ScreenRect.Width) * -1;
                    Y = ((MaxImageSize.Height * Scale) - ScreenRect.Height) * -1;
                    break;
                case Position.Right:
                    X = ((MaxImageSize.Width * Scale) - ScreenRect.Width) * -1;
                    break;
                case Position.Top:
                    Y = 0;
                    break;
                case Position.TopLeft:
                    X = 0;
                    Y = 0;
                    break;
                case null:
                    break;
            }
        });

        public DelegateCommand<Position?> MovePreviewImageCommand => new DelegateCommand<Position?>((param) =>
        {
            switch (param)
            {
                case Position.Left:
                    X += 20;
                    break;
                case Position.Bottom:
                    Y += 20;
                    break;
                case Position.Right:
                    X -= 20;
                    break;
                case Position.Top:
                    Y -= 20;
                    break;
            }
        });

        public DelegateCommand ResetPositionCommand => new DelegateCommand(() =>
        {
            RelativePosition = default;
        });

        public DelegateCommand ResetPositionAndScaleCommand => new DelegateCommand(() =>
        {
            Scale = 1.0;
            RelativePosition = default;
        });

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });

            var notNulls = ImageWrappers.Where(w => w != null).ToList();

            if (notNulls.Any())
            {
                MaxImageSize = new Size(
                    notNulls.Max(w => w.ImageFileInfo.Width),
                    notNulls.Max(w => w.ImageFileInfo.Height));
            }

            RaisePropertyChanged(nameof(MaxImageSize));
        }

        public void SetImageWrappers(IEnumerable<ImageWrapper> imageWrappers)
        {
            var ws = imageWrappers.ToList();
            SetImageWrappers(ws[0], ws[1], ws[2], ws[3]);
        }

        /// <summary>
        /// コンテナに入力されている画像ファイル名のリストを取得します。
        /// </summary>
        /// <returns>リストの要素数は常に 4 です。ファイル名が入力されていない箇所には string.Empty が入力されます。</returns>
        public List<string> GetImageFileNames()
        {
            return ImageWrappers
                .Select(w => w != null
                    ? Path.GetFileNameWithoutExtension(w.ImageFileInfo.FileInfo.Name)
                    : string.Empty)
                .ToList();
        }

        /// <summary>
        /// このオブジェクトのディープコピーを取得します。
        /// </summary>
        /// <returns>このオブジェクトのプロパティがコピーされた新しいオブジェクト。</returns>
        public PreviewContainer Clone()
        {
            return new PreviewContainer
            {
                PreviewScale = PreviewScale,
                ScreenRect = new BindableRect(ScreenRect.X, ScreenRect.Y, ScreenRect.Width, ScreenRect.Height),
                Scale = Scale,
                ImageWrappers = new ObservableCollection<ImageWrapper>(ImageWrappers),
                MaxImageSize = MaxImageSize,
                X = X,
                Y = Y,
                TagType = TagType,
                TagId = TagId,
            };
        }

        /// <summary>
        /// 入力された角度と距離に基づいて、画像を移動させます。
        /// </summary>
        /// <param name="degree">度単位で角度を入力します。</param>
        /// <param name="distance">移動させる距離を入力します。</param>
        public void MoveImage(double degree, double distance)
        {
            if (distance == 0)
            {
                return;
            }

            var radian = degree * (Math.PI / 180.0);
            var deltaX = distance * Math.Cos(radian);
            var deltaY = distance * Math.Sin(radian);

            X += deltaX;
            Y -= deltaY;
        }

        /// <summary>
        /// 現在のImageWrappersをコピーして内部に保存します。
        /// LoadImageWrappers() で保存したリストを取得できます。
        /// </summary>
        public void SaveStatus()
        {
            originalImageWrappers = new List<ImageWrapper>(ImageWrappers);
        }

        public IEnumerable<ImageWrapper> LoadImageWrappers()
        {
            return originalImageWrappers;
        }
    }
}