using System.IO;
using System.Linq;
using ImageChecker_3.Models;
using ImageChecker_3.Models.Images;
using Prism.Mvvm;

namespace ImageChecker_3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            ImageWrapperProvider = new DummyImageProvider();

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('a').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('b').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('c').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('d').FirstOrDefault());
        }

        public MainWindowViewModel(IImageWrapperProvider imageWrapperProvider)
        {
            ImageWrapperProvider = imageWrapperProvider;

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('a').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('b').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('c').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('d').FirstOrDefault());
        }

        public TitleBarText TitleBarText { get; set; } = new ();

        public PreviewContainer PreviewContainer { get; private set; } = new ();

        private IImageWrapperProvider ImageWrapperProvider { get; set; }

        /// <summary>
        /// 指定されたディレクトリのパスから画像ファイルを読み込みます。
        /// </summary>
        /// <param name="directoryPath">画像ファイルを含むディレクトリのパスを指定します。</param>
        public void LoadImages(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                return;
            }

            ImageWrapperProvider.Load(directoryPath);

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('a').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('b').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('c').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('d').FirstOrDefault());
        }
    }
}