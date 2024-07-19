using System;
using ImageChecker_3.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ImageChecker_3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingPageViewModel : BindableBase, IDialogAware
    {
        private AppSettings originalSettings;

        public event Action<IDialogResult> RequestClose;

        public AppSettings AppSettings { get; set; }

        public string Title => string.Empty;

        public DelegateCommand CloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            if (originalSettings == null || AppSettings == null)
            {
                return;
            }

            if (!originalSettings.IsEqualTo(AppSettings))
            {
                AppSettings.SaveToFile(AppSettings.SettingFileName);
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            AppSettings = parameters.GetValue<AppSettings>(nameof(AppSettings));

            originalSettings = new AppSettings
            {
                ImageTagText = AppSettings.ImageTagText,
                DrawTagText = AppSettings.DrawTagText,
                AnimationImageTagText = AppSettings.AnimationImageTagText,
                AnimationDrawTagText = AppSettings.AnimationDrawTagText,
            };
        }
    }
}