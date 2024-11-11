using System;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.Models.Images
{
    /// <summary>
    /// 画像をスライドして動かす機能を管理するクラスです。
    /// </summary>
    public class SlideController : BindableBase
    {
        private int distance;
        private int degree;
        private int duration;

        public SlideController(PreviewContainer container)
        {
            PreviewContainer = container;
        }

        public PreviewContainer PreviewContainer { get; set; }

        public int Distance { get => distance; set => SetProperty(ref distance, value); }

        public int Degree { get => degree; set => SetProperty(ref degree, value); }

        public int Duration { get => duration; set => SetProperty(ref duration, value); }

        public DelegateCommand MovePreviewImageCommand => new DelegateCommand(() =>
        {
            if (PreviewContainer == null)
            {
                return;
            }

            PreviewContainer.MoveImage(Degree, Distance);
        });

        public DelegateCommand ReverseMovePreviewImageCommand => new DelegateCommand(() =>
        {
            if (PreviewContainer == null)
            {
                return;
            }

            PreviewContainer.MoveImage(Degree + 180, Distance);
        });

        public DelegateCommand<string> ChangeDegreeCommand => new (param =>
        {
            if (param == null)
            {
                return;
            }

            var value = int.Parse(param);
            var d = Degree + value;

            switch (d)
            {
                case >= 360:
                    Degree = d % 360;
                    return;
                case < 0:
                    var abs = Math.Abs(d) % 360;
                    Degree = 360 - abs;
                    return;
            }

            Degree = d;
        });
    }
}