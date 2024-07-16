using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    public class ImageFileInfo : BindableBase
    {
        private double width;
        private double height;
        private Int32Rect opaqueRange;
        private ImageSource croppedImage = new BitmapImage();

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

        public char KeyChar { get; set; }

        public FileInfo FileInfo { get; set; }

        public Int32Rect OpaqueRange
        {
            get => opaqueRange;
            set
            {
                if (opaqueRange == value)
                {
                    return;
                }

                opaqueRange = value;

                var bitmap = new BitmapImage(new Uri(FileInfo.FullName));
                var croppedBitmap = new CroppedBitmap(bitmap, opaqueRange);
                CroppedImage = croppedBitmap;
            }
        }

        /// <summary>
        /// FileInfo の示すパスの画像が格納されます。
        /// もし画像に完全に透明な領域が存在する場合、それをカットした ImageSource が格納されます。
        /// </summary>
        public ImageSource CroppedImage
        {
            get => croppedImage;
            private set => SetProperty(ref croppedImage, value);
        }

        public override string ToString()
        {
            return FileInfo.Name;
        }

        /// <summary>
        /// 指定したパスの画像ファイルを読み込み、このオブジェクトのプロパティに情報をセットします。
        /// </summary>
        /// <param name="imageFilePath">画像ファイルの絶対パス</param>
        private void LoadImageInfo(string imageFilePath)
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
            CroppedImage = bitmap;

            // 画像ファイル名が特定の命名規則に沿っていれば、ファイル名に関するプロパティも設定する。
            if (!Regex.Match(FileInfo.Name, @"[ABCD]\d\d\d\d").Success)
            {
                return;
            }

            IsMatchingNamingRule = true;
            var matches = Regex.Matches(FileInfo.Name, @"([ABCD])(\d\d)(\d\d)");
            KeyChar = matches[0].Groups[1].Value.FirstOrDefault();
            Index = int.Parse(matches[0].Groups[2].Value);
            SubIndex = int.Parse(matches[0].Groups[3].Value);
        }
    }
}