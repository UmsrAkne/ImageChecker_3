using System.IO;
using System.Linq;
using System.Windows;
using ImageChecker_3.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace ImageChecker_3.Views.Behaviors
{
    public class DragAndDropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (((Window)sender).DataContext is not MainWindowViewModel vm)
            {
                return;
            }

            if (files?.Length == 1 && Directory.Exists(files.First()))
            {
                // files の要素数が 1 でドロップアイテムがディレクトリならば、画像用ディレクトリがドロップされたということなので
                // ディレクトリの内容をロードして渡す。
                vm.LoadImages(files.FirstOrDefault());
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}