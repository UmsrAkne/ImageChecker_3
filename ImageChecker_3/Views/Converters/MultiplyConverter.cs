using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ImageChecker_3.Views.Converters
{
    /// <summary>
    /// 複数の <see cref="double"/> 値を受け取り、それらの積を計算して文字列として返す <see cref="IMultiValueConverter"/> の実装です。 <br/>
    /// PreviewScaleMultiConverter と似た機能を持ちますが、あちらは Convert の結果を double で返す一方、このクラスは string を返します。
    /// TextBlock.Text に変換結果を表示するためのクラスです。
    /// </summary>
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var doubles = values.OfType<double>();
            var enumerable = doubles as double[] ?? doubles.ToArray();
            if (enumerable.Length != 0)
            {
                return enumerable.Aggregate((now, next) => now * next).ToString(CultureInfo.CurrentCulture);
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}