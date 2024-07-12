using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class ImageContainer : BindableBase
    {
        private readonly string keyChar;
        private List<ImageWrapper> filteredFiles = new List<ImageWrapper>();
        private bool drawing;
        private int selectedIndex;
        private ImageWrapper currentFile;
        private bool isEnabled = true;
        private List<ImageWrapper> imageWrappers = new List<ImageWrapper>();

        public ImageContainer(string keyChar)
        {
            this.keyChar = keyChar;
        }

        public event EventHandler CurrentFileChanged;

        public List<ImageWrapper> FilteredFiles
        {
            get => filteredFiles;
            private set => SetProperty(ref filteredFiles, value);
        }

        public ImageWrapper CurrentFile
        {
            get => IsEnabled ? currentFile : null;
            set
            {
                if (currentFile != null)
                {
                    currentFile.IsSelected = false;
                }

                if (value != null)
                {
                    value.IsSelected = true;
                }

                if (SetProperty(ref currentFile, value))
                {
                    CurrentFileChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool Drawing { get => drawing; set => SetProperty(ref drawing, value); }

        public int SelectedIndex { get => selectedIndex; set => SetProperty(ref selectedIndex, value); }

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (SetProperty(ref isEnabled, value))
                {
                    CurrentFileChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public List<ImageWrapper> ImageWrappers
        {
            private get => imageWrappers;
            set
            {
                if (SetProperty(ref imageWrappers, value))
                {
                    CurrentFile = value.FirstOrDefault();
                    IsEnabled = value.Count > 0;
                    if (ImageWrappers.Count == 0)
                    {
                        Drawing = false;
                    }
                }
            }
        }

        public void SelectSameGroupImages(ImageWrapper baseImageFile)
        {
            if (keyChar == "A")
            {
                return;
            }

            var groupIndex = baseImageFile.ImageFileInfo.Index;

            FilteredFiles = ImageWrappers.Where(imageFile => imageFile.ImageFileInfo.Index == groupIndex).ToList();
            CurrentFile = FilteredFiles.FirstOrDefault();
        }

        public string GetCurrentFileName()
        {
            if (!Drawing || CurrentFile == null)
            {
                return string.Empty;
            }

            return Path.GetFileNameWithoutExtension(CurrentFile.ImageFileInfo.FileInfo.FullName);
        }

        public void SetImageByName(string name)
        {
            Drawing = !string.IsNullOrWhiteSpace(name);
            if (Drawing)
            {
                CurrentFile =
                    FilteredFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.ImageFileInfo.FileInfo.Name) == name);
            }
        }
    }
}