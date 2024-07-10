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
        }

        public MainWindowViewModel(IImageWrapperProvider imageWrapperProvider)
        {
            ImageWrapperProvider = imageWrapperProvider;
        }

        public TitleBarText TitleBarText { get; set; } = new ();

        private IImageWrapperProvider ImageWrapperProvider { get; set; }
    }
}