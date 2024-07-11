using System.Collections.Generic;

namespace ImageChecker_3.Models.Images
{
    public interface IImageWrapperProvider
    {
        List<ImageWrapper> GetImageWrappers(char keyChar);
    }
}