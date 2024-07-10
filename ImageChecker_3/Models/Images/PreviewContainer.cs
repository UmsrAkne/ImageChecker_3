using System.Collections.ObjectModel;
using System.Windows;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer : BindableBase
    {
        private double previewScale = 0.5;

        public ObservableCollection<ImageWrapper> ImageWrappers { get; set; } = new ();

        public Rect ScreenSize { get; set; }

        public Rect PreviewRect { get; set; } = new Rect(0, 0, 1280, 720);

        public double PreviewScale { get => previewScale; set => SetProperty(ref previewScale, value); }

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });
        }
    }
}