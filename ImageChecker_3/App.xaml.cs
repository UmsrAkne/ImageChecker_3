using System.Windows;
using ImageChecker_3.Models.Images;
using ImageChecker_3.Views;
using Prism.Ioc;

namespace ImageChecker_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #if DEBUG
                containerRegistry.Register<IImageWrapperProvider, DummyImageProvider>();
            #else
                containerRegistry.Register<IImageWrapperProvider, ImageWrapperProvider>();
            #endif
        }
    }
}