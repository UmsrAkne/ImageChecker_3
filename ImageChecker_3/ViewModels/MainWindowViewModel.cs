using ImageChecker_3.Models;
using Prism.Mvvm;

namespace ImageChecker_3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public TitleBarText TitleBarText { get; set; } = new ();

        public MainWindowViewModel()
        {
        }
    }
}