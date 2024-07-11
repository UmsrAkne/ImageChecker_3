using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class ImageFileInfo : BindableBase
    {
        private double width;
        private double height;

        public ImageFileInfo()
        {
        }

        public ImageFileInfo(string filePath)
            : this()
        {
            if (File.Exists(filePath))
            {
                LoadImageInfo(filePath);
            }
        }

        public double Width { get => width; set => SetProperty(ref width, value); }

        public double Height { get => height; set => SetProperty(ref height, value); }

        public bool IsMatchingNamingRule { get; private set; }

        public int Index { get; set; }

        public int SubIndex { get; set; }

        public FileInfo FileInfo { get; set; }

        public override string ToString()
        {
            return FileInfo.Name;
        }

        /// <summary>
        /// 指定したパスの画像ファイルを読み込み、このオブジェクトのプロパティに情報をセットします。
        /// </summary>
        /// <param name="imageFilePath">画像ファイルの絶対パス</param>
        public void LoadImageInfo(string imageFilePath)
        {
            FileInfo = new FileInfo(imageFilePath);

            // BitmapImage を使ってがぞの読み込み・初期化・データの取得
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imageFilePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // 画像のサイズを取得するために、即座に読み込む必要がある
            bitmap.EndInit();

            Width = bitmap.PixelWidth;
            Height = bitmap.PixelHeight;

            // 画像ファイル名が特定の命名規則に沿っていれば、ファイル名に関するプロパティも設定する。
            if (!Regex.Match(FileInfo.Name, @"[ABCD]\d\d\d\d").Success)
            {
                return;
            }

            IsMatchingNamingRule = true;
            var matches = Regex.Matches(FileInfo.Name, @"[ABCD](\d\d)(\d\d)");
            Index = int.Parse(matches[0].Groups[1].Value);
            SubIndex = int.Parse(matches[0].Groups[2].Value);
        }
    }
}