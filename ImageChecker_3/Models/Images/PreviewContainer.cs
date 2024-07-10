using System.Collections.ObjectModel;
using System.Windows;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer : BindableBase
    {
        private double previewScale = 0.5;

        public ObservableCollection<ImageWrapper> ImageWrappers { get; set; } = new ();

        /// <summary>
        /// 実際のプレイ時の画面のサイズを表します。
        /// </summary>
        public Rect ScreenRect { get; set; } = new Rect(0, 0, 1280, 720);

        /// <summary>
        /// 実際の画面のサイズに対する、プレビュー領域のサイズ比です。
        /// 例えば、このプロパティの値が 0.5 ならば、画面サイズに対してプレビュー領域が半分の大きさとなります。
        /// </summary>
        public double PreviewScale { get => previewScale; set => SetProperty(ref previewScale, value); }

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });
        }
    }
}