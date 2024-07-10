using System.Collections.ObjectModel;
using System.Windows;

namespace ImageChecker_3.Models.Images
{
    public class PreviewContainer
    {
        public ObservableCollection<ImageWrapper> ImageWrappers { get; set; } = new ();
        
        public Rect ScreenSize { get; set; }

        public Rect PreviewSize { get; set; } = new Rect(0, 0, 640, 360);

        public void SetImageWrappers(ImageWrapper a, ImageWrapper b, ImageWrapper c, ImageWrapper d)
        {
            ImageWrappers.Clear();
            ImageWrappers.AddRange(new[] { a, b, c, d, });
        }
    }
}