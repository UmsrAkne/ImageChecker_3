using System.Windows;

namespace ImageChecker_3.Models.Images
{
    public class ImageWrapper
    {
        public ImageWrapper(ImageFileInfo imageFileInfo)
        {
            ImageFileInfo = imageFileInfo;
        }

        public Point Position { get; set; }

        public ImageFileInfo ImageFileInfo { get; set; }
    }
}