using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ImageChecker_3.Models;
using ImageChecker_3.Models.Images;
using ImageChecker_3.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker_3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // DI コンテナにより生成されるクラスのため、手動によるインスタンス化はしない。
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private GridLength fourthColumnLength;

        public MainWindowViewModel()
        {
            ImageWrapperProvider = new DummyImageProvider();

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('B').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('C').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('D').FirstOrDefault());

            for (var i = 0; i < 10; i++)
            {
                var p = PreviewContainer.Clone();
                p.PreviewScale = 0.1;
                PreviewContainerHistory.Insert(0, p);
            }

            LoadImages(string.Empty);
            FourthColumnLength = new GridLength(0);
        }

        public MainWindowViewModel(IImageWrapperProvider imageWrapperProvider, IDialogService dialogService)
        {
            ImageWrapperProvider = imageWrapperProvider;

            PreviewContainer.SetImageWrappers(
                ImageWrapperProvider.GetImageWrappers('A').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('B').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('C').FirstOrDefault(),
                ImageWrapperProvider.GetImageWrappers('D').FirstOrDefault());

            LoadImages(string.Empty);
            FourthColumnLength = new GridLength(1.0, GridUnitType.Star);
            AppSettings = AppSettings.LoadFromFile(AppSettings.SettingFileName);
            TagGenerator.SetSettings(AppSettings);

            TagGenerator.TagGenerated += (_, _) =>
            {
                var p = PreviewContainer.Clone();
                p.SaveStatus();
                p.PreviewScale = 0.1;
                if (p.TagType is TagType.Draw or TagType.AnimationDraw)
                {
                    var l = p.ImageWrappers.ToList();
                    l[0] = null;
                    p.SetImageWrappers(l);
                }

                PreviewContainerHistory.Insert(0, p);
            };

            ImageContainers.First().CurrentFileChanged += (sender, _) =>
            {
                foreach (var ic in ImageContainers.Where(c => c.KeyChar.First() != 'A'))
                {
                    ic.SelectSameGroupImages(((ImageContainer)sender)?.CurrentFile);
                }
            };

            this.dialogService = dialogService;
        }

        public TitleBarText TitleBarText { get; set; } = new ();

        public List<ImageContainer> ImageContainers { get; private set; }
            = new[] { "A", "B", "C", "D", }.Select(key => new ImageContainer(key)).ToList();

        public TagGenerator TagGenerator { get; private set; } = new ();

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

        public ObservableCollection<PreviewContainer> PreviewContainerHistory { get; set; } = new ();

        /// <summary>
        /// ImageContainers の内容に応じて、PreviewImageContainer を更新します。
        /// </summary>
        public DelegateCommand UpdatePreviewImagesCommand => new DelegateCommand(() =>
        {
            PreviewContainer.SetImageWrappers(
                ImageContainers.Select(c => c.IsEnabled ? c.CurrentFile : null));
        });

        public DelegateCommand ShowSettingPageCommand => new DelegateCommand(() =>
        {
            var param = new DialogParameters { { nameof(AppSettings), AppSettings }, };
            dialogService.ShowDialog(nameof(SettingPage), param, (_) => { });
        });

        /// <summary>
        /// 現在のPreviewContainerの状態を、コマンドパラメーターに与えられたオブジェクトに基づいて書き換えます。
        /// </summary>
        public DelegateCommand<PreviewContainer> RestorePreviewContainerCommand => new DelegateCommand<PreviewContainer>((param) =>
        {
            var images = param.LoadImageWrappers().ToList();
            PreviewContainer.SetImageWrappers(images);
            PreviewContainer.Scale = param.Scale;
            PreviewContainer.RelativePosition = param.RelativePosition;
            PreviewContainer.ScreenRect = param.ScreenRect.Clone();

            for (var i = 0; i < ImageContainers.Count; i++)
            {
                ImageContainers[i].CurrentFile = images[i];
            }
        });

        private AppSettings AppSettings { get; init; }

        private IImageWrapperProvider ImageWrapperProvider { get; set; }

        /// <summary>
        /// 指定されたディレクトリのパスから画像ファイルを読み込みます。その後、非同期で透明領域をカットした画像のプレビュー生成します。
        /// </summary>
        /// <param name="directoryPath">画像ファイルを含むディレクトリのパスを指定します。</param>
        /// <returns>非同期処理を表すタスク。</returns>
        public async Task LoadImagesAsync(string directoryPath)
        {
            LoadImages(directoryPath);

            var ws = new List<ImageWrapper>();
            ws.AddRange(ImageWrapperProvider.GetImageWrappers('A'));
            ws.AddRange(ImageWrapperProvider.GetImageWrappers('B'));
            ws.AddRange(ImageWrapperProvider.GetImageWrappers('C'));
            ws.AddRange(ImageWrapperProvider.GetImageWrappers('D'));

            foreach (var w in ws.Where(w => w.ImageFileInfo.FileInfo.Exists))
            {
                w.ImageFileInfo.OpaqueRange =
                    await ImageBoundsCalculator.GetOpaquePixelBoundsAsync(w.ImageFileInfo.FileInfo.FullName);
            }
        }

        /// <summary>
        /// 指定されたディレクトリのパスから画像ファイルを読み込みます。
        /// </summary>
        /// <param name="directoryPath">画像ファイルを含むディレクトリのパスを指定します。</param>
        private void LoadImages(string directoryPath)
        {
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