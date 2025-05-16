using System;
using ImageChecker_3.Tags;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageChecker_3.Images
{
    /// <summary>
    /// 画像をスライドして動かす機能を管理するクラスです。
    /// </summary>
    public class SlideController : BindableBase
    {
        private int distance;
        private int degree;
        private int duration = 50;

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

        /// <summary>
        /// パラメーターに入力された string を数値に変換し、 Degree プロパティに加算します。
        /// </summary>
        /// <remarks>
        /// Degree に数値を加算した結果が 0 未満である、または 360 より大きい場合、 以下のように調節されます。<br/>
        /// Degree = 370° の場合 : Degree = 10° に調節。<br/>
        /// Degree = -10° の場合 : Degree = 350° に調節。<br/>
        /// パラメーターとして受け取った文字列が null または数値以外の文字列が指定された場合、本メソッドは何も行わずに終了します。
        /// </remarks>
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

        /// <summary>
        /// 距離を変更するコマンドです。<br/>
        /// 指定されたパラメータの値を現在の距離に加算し、結果を更新します。<br/>
        /// 更新後の距離は負の値にならないように制御されます。
        /// </summary>
        /// <remarks>
        /// パラメーターには変更する距離を示す文字列を入力します。<br/>
        /// 整数値に変換可能な文字列を指定してください。<br/>
        /// パラメーターにnull が渡された場合、何も行いません。
        /// </remarks>
        public DelegateCommand<string> ChangeDistanceCommand => new (distanceDelta =>
        {
            if (distanceDelta == null)
            {
                return;
            }

            var value = int.Parse(distanceDelta);
            var d = Distance + value;
            Distance = Math.Max(0, d);
        });

        /// <summary>
        /// Duration を変更するコマンドです。<br/>
        /// 指定されたパラメータの値を現在のDurationに加算し、結果を更新します。<br/>
        /// 更新後、Durationは負の値にならないように制御されます。
        /// </summary>
        /// <remarks>
        /// パラメーターには変更する距離を示す文字列を入力します。<br/>
        /// 整数値に変換可能な文字列を指定してください。<br/>
        /// パラメーターに null や空文字が渡された場合、何も行いません。
        /// </remarks>
        public DelegateCommand<string> ChangeDurationCommand => new DelegateCommand<string>((durationDelta) =>
        {
            if (string.IsNullOrWhiteSpace(durationDelta))
            {
                return;
            }

            var value = int.Parse(durationDelta);
            var d = Duration + value;
            Duration = Math.Max(0, d);
        });

        /// <summary>
        /// 入力されたスライドタグのプロパティをエリアのテキストボックスに反映します。
        /// </summary>
        public DelegateCommand<SlideTag> ApplySlideTagCommand => new ((slideTag) =>
        {
            if (slideTag == null)
            {
                return;
            }

            Duration = slideTag.Duration;
            Distance = slideTag.Distance;
            Degree = slideTag.Degree;
        });
    }
}