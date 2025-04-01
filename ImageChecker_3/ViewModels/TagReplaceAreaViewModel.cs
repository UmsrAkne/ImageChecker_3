using System.Windows;
using ImageChecker_3.Models.Images;
using ImageChecker_3.Models.Tags;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TagReplaceAreaViewModel : BindableBase
    {
        private string inputText = string.Empty;

        public PreviewContainer PreviewContainer { private get; set; }

        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }

        public DelegateCommand ReplaceAndCopyCommand => new (() =>
        {
            if (PreviewContainer == null || string.IsNullOrWhiteSpace(InputText))
            {
                return;
            }

            Clipboard.SetText(TagReplacer.ReplaceImageNames(InputText, PreviewContainer));
        });
    }
}