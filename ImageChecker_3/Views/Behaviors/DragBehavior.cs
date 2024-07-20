using System;
using System.Windows;
using System.Windows.Input;
using ImageChecker_3.Models.Images;
using Microsoft.Xaml.Behaviors;

namespace ImageChecker_3.Views.Behaviors
{
    public class DragBehavior : Behavior<PreviewScreenArea>
    {
        private Point lastPoint = new (0, 0);
        private PreviewContainer previewContainer;

        protected override void OnAttached()
        {
            base.OnAttached();
            previewContainer = AssociatedObject.DataContext as PreviewContainer;
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
            AssociatedObject.MouseUp += AssociatedObjectOnMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
            AssociatedObject.MouseUp -= AssociatedObjectOnMouseUp;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (lastPoint == new Point(0, 0))
                {
                    // マウスのボタンを押した一発目の状態。この場合は現在の値を入れて、次からは一つ前の座標に基づき移動の処理をする。
                    lastPoint = e.GetPosition(AssociatedObject);
                    return;
                }

                var distance = lastPoint - e.GetPosition(AssociatedObject);
                previewContainer.X -= (int)distance.X / previewContainer.PreviewScale;
                previewContainer.Y -= (int)distance.Y / previewContainer.PreviewScale;

                lastPoint = e.GetPosition(AssociatedObject);
            }

            if (e.LeftButton == MouseButtonState.Released)
            {
                lastPoint = new Point(0, 0);
            }
        }

        private void AssociatedObjectOnMouseUp(object sender, MouseEventArgs e)
        {
            previewContainer.X = Math.Round(previewContainer.X / 10) * 10;
            previewContainer.Y = Math.Round(previewContainer.Y / 10) * 10;
        }
    }
}