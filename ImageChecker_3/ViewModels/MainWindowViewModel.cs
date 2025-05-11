using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ImageChecker_3.Images;
using ImageChecker_3.Models;
using ImageChecker_3.Tags;
using ImageChecker_3.Utils;
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
            SlideController = new SlideController(PreviewContainer);
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
            SlideController = new SlideController(PreviewContainer);

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

            TagReplaceAreaViewModel.PreviewContainer = PreviewContainer;

            this.dialogService = dialogService;
        }

        public AppVersionInfo TitleBarText { get; set; } = new ();

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

        public PreviewContainerHistory PreviewContainerHistory { get; set; } = new ();

        public SlideController SlideController { get; private set; }

        public TagReplaceAreaViewModel TagReplaceAreaViewModel { get; init; } = new ();

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

            TagGenerator.SetSettings(AppSettings);
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

        /// <summary>
        /// 現在のPreviewContainerの位置を、コマンドパラメーターに与えられたオブジェクトに基づいて書き換えます。
        /// </summary>
        public DelegateCommand<PreviewContainer> RestorePositionFromPcCommand => new DelegateCommand<PreviewContainer>((param) =>
        {
            PreviewContainer.Scale = param.Scale;
            PreviewContainer.RelativePosition = param.RelativePosition;
            PreviewContainer.ScreenRect = param.ScreenRect.Clone();
        });

        public DelegateCommand<PreviewContainer> DeleteHistoryCommand => new DelegateCommand<PreviewContainer>((param) =>
        {
            if (param == null)
            {
                return;
            }

            var index = PreviewContainerHistory.Items.IndexOf(param);
            if (index != -1)
            {
                PreviewContainerHistory.RemoveAt(index);
            }
        });

        public DelegateCommand<object> MoveImageContainerCursorCommandCommand => new DelegateCommand<object>((param) =>
        {
            var keyStr = (string)param;
            if (string.IsNullOrWhiteSpace(keyStr))
            {
                return;
            }

            var target = ImageContainers.FirstOrDefault(c => c.KeyChar == keyStr.ToUpper());
            if (target == null)
            {
                return;
            }

            var changeAmount = char.IsLower(keyStr[0]) ? 1 : -1;
            target.SelectedIndex += changeAmount;
        });

        public DelegateCommand ShowTagLoadPageCommand => new DelegateCommand(() =>
        {
            var param = new DialogParameters();
            dialogService.ShowDialog(nameof(TagLoadPage), param, result =>
            {
                if (result.Result != ButtonResult.OK)
                {
                    return;
                }

                var tag = result.Parameters.GetValue<object>("tag");
                if (tag is ImageTag imageTag)
                {
                    var imageFileNames = new[] { imageTag.A, imageTag.B, imageTag.C, imageTag.D, };
                    var wrappers = new List<ImageWrapper>() { null, null, null, null, };
                    for (var i = 0; i < imageFileNames.Length; i++)
                    {
                        var w = ImageContainers[i].FilteredFiles.FirstOrDefault(w =>
                            w != null && w.ImageFileInfo.FileNameWithoutExtension == imageFileNames[i]);

                        wrappers[i] = w;
                        ImageContainers[i].CurrentFile = w;
                    }

                    PreviewContainer.SetImageWrappers(wrappers);

                    PreviewContainer.Scale = imageTag.Scale;
                    PreviewContainer.RelativePosition = new Point(imageTag.X, imageTag.Y);
                }
            });
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
        /// 指定されたディレクトリのパスから画像ファイルを読み込みます。<br/>
        /// また、PreviewContainer.ScreenRect.Width がデフォルト値のとき、読み込んだ画像から幅を取得し、値をセットします。
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

            if (PreviewContainer.ScreenRect.Width == 0)
            {
                PreviewContainer.ScreenRect.Width = ImageWrapperProvider.GetBaseWidth();
            }
        }
    }
}