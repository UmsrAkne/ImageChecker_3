using System.Collections.Generic;
using System.IO;

namespace ImageChecker_3.Models.Images
{
    public class DummyImageProvider : IImageWrapperProvider
    {
        private readonly Dictionary<char, List<ImageWrapper>> imageWrappers = new ()
        {
            {
                'A', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\A0101.png") { Width = 1280, Height = 720, }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 2, FileInfo = new FileInfo("A0102.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 3, FileInfo = new FileInfo("A0103.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 4, FileInfo = new FileInfo("A0104.png"),
                    }),
                }
            },
            {
                'B', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 1, FileInfo = new FileInfo("B0101.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 2, FileInfo = new FileInfo("B0102.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 3, FileInfo = new FileInfo("B0103.png"),
                    }),
                }
            },
            {
                'C', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 1, FileInfo = new FileInfo("C0101.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 2, FileInfo = new FileInfo("C0102.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 3, FileInfo = new FileInfo("C0103.png"),
                    }),
                }
            },
            {
                'D', new List<ImageWrapper>()
                {
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 1, FileInfo = new FileInfo("D0101.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 2, FileInfo = new FileInfo("D0102.png"),
                    }),
                    new (new ImageFileInfo()
                    {
                        Width = 1280, Height = 720, Index = 1, SubIndex = 3, FileInfo = new FileInfo("D0103.png"),
                    }),
                }
            },
        };

        public List<ImageWrapper> GetImageWrappers(char keyChar)
        {
            System.Diagnostics.Debug.WriteLine($"--------------------(DummyImageProvider : 46)");
            System.Diagnostics.Debug.WriteLine($"keyChar = {keyChar} (DummyImageProvider : 46)");
            System.Diagnostics.Debug.WriteLine($"{imageWrappers.GetValueOrDefault(keyChar).Count} 個の要素をもつリストを返します。(DummyImageProvider : 47)");
            System.Diagnostics.Debug.WriteLine($"--------------------(DummyImageProvider : 49)");
            return imageWrappers.GetValueOrDefault(keyChar);
        }

        public void Load(string directoryPath)
        {
            System.Diagnostics.Debug.WriteLine($"Load() が実行されました。指定パス : {directoryPath}(DummyImageProvider : 55)");
        }
    }
}