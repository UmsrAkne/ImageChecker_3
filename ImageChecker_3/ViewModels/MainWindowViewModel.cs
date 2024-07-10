using Prism.Mvvm;

namespace ImageChecker_3.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string title = "ImageCheckerV3";

        public string Title { get => title; set => SetProperty(ref title, value); }

        public MainWindowViewModel()
        {
        }
    }
}