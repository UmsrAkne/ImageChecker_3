using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ImageChecker_3.Models;
using ImageChecker_3.Models.Images;
using Prism.Mvvm;

namespace ImageChecker_3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // DI コンテナにより生成されるクラスのため、手動によるインスタンス化はしない。
    public class MainWindowViewModel : BindableBase
    {
        private GridLength fourthColumnLength;

        public MainWindowViewModel()
        {
            ImageWrapperProvider = new DummyImageProvider();

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('B').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('C').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('D').FirstOrDefault());

            FourthColumnLength = new GridLength(0);
        }

        public MainWindowViewModel(IImageWrapperProvider imageWrapperProvider)
        {
            ImageWrapperProvider = imageWrapperProvider;

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('B').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('C').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('D').FirstOrDefault());

            FourthColumnLength = new GridLength(1.0, GridUnitType.Star);
        }

        public TitleBarText TitleBarText { get; set; } = new ();

        public List<ImageContainer> ImageContainers { get; private set; }
            = new[] { "A", "B", "C", "D", }.Select(key => new ImageContainer(key)).ToList();

        /// <summary>
        /// Grid の ４列目の幅を設定するためのプロパティです。
        /// 表示されている・表示されておらずスペースも確保されていない のどちらかの状態です。
        /// </summary>
        public GridLength FourthColumnLength
        {
            get => fourthColumnLength;
            private set => SetProperty(ref fourthColumnLength, value);
        }

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
                ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('B').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('C').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('D').FirstOrDefault());

            foreach (var ic in ImageContainers)
            {
                ic.ImageWrappers = ImageWrapperProvider.GetImageWrappers(ic.KeyChar.First());
                ic.SelectSameGroupImages(ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault());
            }
        }
    }
}