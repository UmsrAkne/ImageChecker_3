using System.Windows;
using Prism.Mvvm;

namespace ImageChecker_3.Images
{
    public class ImageWrapper : BindableBase
    {
        private bool isSelected;

        public ImageWrapper(ImageFileInfo imageFileInfo)
        {
            ImageFileInfo = imageFileInfo;
        }

        public Point Position { get; set; }

        public ImageFileInfo ImageFileInfo { get; set; }

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
    }
}