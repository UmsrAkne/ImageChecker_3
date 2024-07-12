using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageChecker_3.Models.Images
{
    public class ImageWrapperProvider : IImageWrapperProvider
    {
        private List<ImageWrapper> imageWrappers = new ();

        public List<ImageWrapper> GetImageWrappers(char keyChar)
        {
            return imageWrappers.Where(w => w.ImageFileInfo.KeyChar == keyChar).ToList();
        }

        public void Load(string directoryPath)
        {
            imageWrappers = Directory.GetFiles(directoryPath)
                    .Where(p => Path.GetExtension(p) == ".png")
                    .Select(p => new ImageWrapper(new ImageFileInfo(p)))
                    .ToList();
        }
    }
}