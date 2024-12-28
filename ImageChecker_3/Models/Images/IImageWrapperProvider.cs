using System.Collections.Generic;

namespace ImageChecker_3.Models.Images
{
    public interface IImageWrapperProvider
    {
        List<ImageWrapper> GetImageWrappers(char keyChar);

        /// <summary>
        /// 指定されたディレクトリの中の画像ファイルをロードします。
        /// ロードした画像はクラスの内部に ImageWrapper に変換されて格納されます。
        /// </summary>
        /// <param name="directoryPath">画像ファイルを含むディレクトリのパスを指定します。</param>
        void Load(string directoryPath);

        int GetBaseWidth();
    }
}