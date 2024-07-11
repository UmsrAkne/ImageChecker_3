using System.Collections.Generic;

namespace ImageChecker_3.Models.Images
{
    public class ImageWrapperProvider : IImageWrapperProvider
    {
        public List<ImageWrapper> GetImageWrappers(char keyChar)
        {
            return new List<ImageWrapper>();
        }
    }
}