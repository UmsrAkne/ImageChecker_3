using System;
using ImageChecker_3.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker_3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TagLoadPageViewModel : BindableBase, IDialogAware
    {
        private string inputText;
        private string parsedResult;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }

        public string ParsedResult { get => parsedResult; private set => SetProperty(ref parsedResult, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ParseTagTextCommand => new DelegateCommand(() =>
        {
            if (InputText.StartsWith("<image"))
            {
                ParsedResult = TagParser.LoadImageTag(InputText).ToString();
            }

            if (InputText.StartsWith("<draw"))
            {
                ParsedResult = TagParser.LoadDrawTag(InputText).ToString();
            }

            if (InputText.StartsWith("<anime") && InputText.Contains(@"name=""slide"""))
            {
                ParsedResult = TagParser.LoadSlideTag(InputText).ToString();
            }
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}