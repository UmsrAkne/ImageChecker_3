using System.Windows.Controls;
using System.Windows.Input;
using ImageChecker_3.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace ImageChecker_3.Behaviors
{
    public class SpringButtonBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObjectOnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObjectOnPreviewMouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObjectOnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= AssociatedObjectOnPreviewMouseLeftButtonUp;
        }

        private void AssociatedObjectOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = AssociatedObject.DataContext;
            if (vm is MainWindowViewModel mvm)
            {
                mvm.SlideController.MovePreviewImageCommand.Execute();
            }
        }

        private void AssociatedObjectOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var vm = AssociatedObject.DataContext;
            if (vm is MainWindowViewModel mvm)
            {
                mvm.SlideController.ReverseMovePreviewImageCommand.Execute();
            }
        }
    }
}