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
            if (InputText.StartsWith("<image") || InputText.StartsWith("<draw"))
            {
                ParsedResult = TagParser.LoadImageTag(InputText).ToString();
            }

            if (InputText.StartsWith("<anime"))
            {
                if (InputText.Contains(@"name=""slide"""))
                {
                    ParsedResult = TagParser.LoadSlideTag(InputText).ToString();
                }

                if (InputText.Contains(@"name=""image""") || InputText.Contains(@"name=""draw"""))
                {
                    ParsedResult = TagParser.LoadImageTag(InputText).ToString();
                }
            }
        });

        public DelegateCommand SendTagCommand => new DelegateCommand(() =>
        {
            var obj = new object();
            if (string.IsNullOrWhiteSpace(ParsedResult))
            {
                if (InputText.StartsWith("<image") || InputText.StartsWith("<draw"))
                {
                    obj = TagParser.LoadImageTag(InputText);
                }

                if (InputText.StartsWith("<anime"))
                {
                    if (InputText.Contains(@"""draw""") || InputText.Contains(@"""image"""))
                    {
                        obj = TagParser.LoadImageTag(InputText);
                    }
                }

                if (InputText.StartsWith("<anime") && InputText.Contains(@"name=""slide"""))
                {
                    obj = TagParser.LoadSlideTag(InputText);
                }
            }

            var result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add("tag", obj);
            RequestClose?.Invoke(result);
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