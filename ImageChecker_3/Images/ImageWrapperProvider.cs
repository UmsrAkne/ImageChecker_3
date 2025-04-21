using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageChecker_3.Images
{
    public class ImageWrapperProvider : IImageWrapperProvider
    {
        private List<ImageWrapper> imageWrappers = new ();

        public List<ImageWrapper> GetImageWrappers(char keyChar)
        {
            return imageWrappers.Where(w => w.ImageFileInfo.KeyChar == keyChar).ToList();
        }

        /// <summary>
        /// 画像をロードします。引数に無効なパスを入力した場合、動作せずにメソッドを終了します。
        /// </summary>
        /// <param name="directoryPath">ディレクトリのパスを入力します。</param>
        public void Load(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                return;
            }

            imageWrappers = Directory.GetFiles(directoryPath)
                    .Where(p => Path.GetExtension(p) == ".png")
                    .Select(p => new ImageWrapper(new ImageFileInfo(p)))
                    .ToList();
        }

        public int GetBaseWidth()
        {
            var baseSizeImage = imageWrappers.FirstOrDefault(w => w.ImageFileInfo.Width >= 1280 && w.ImageFileInfo.Width <= 1520);
            return baseSizeImage == null ? 0 : (int)baseSizeImage.ImageFileInfo.Width;
        }
    }
}