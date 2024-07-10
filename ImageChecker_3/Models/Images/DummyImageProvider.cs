using System.Collections.Generic;

namespace ImageChecker_3.Models.Images
{
    public class DummyImageProvider : IImageWrapperProvider
    {
        private readonly Dictionary<char, List<ImageWrapper>> imageWrappers = new ()
        {
            {
                'a', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\A0101.png") { Width = 1280, Height = 720, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 1, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 2, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 3, }),
                }
            },
            {
                'b', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 1, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 2, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 3, }),
                }
            },
            {
                'c', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 1, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 2, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 3, }),
                }
            },
            {
                'd', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 1, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 2, }),
                    new (new ImageFileInfo() { Width = 1280, Height = 720, Index = 1, SubIndex = 3, }),
                }
            },
        };

        public List<ImageWrapper> GetImageWrappers(char keyChar)
        {
            return imageWrappers.GetValueOrDefault(keyChar);
        }
    }
}