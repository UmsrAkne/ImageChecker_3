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

        private ImageFileInfo ImageFileInfo { get; set; }
    }
}