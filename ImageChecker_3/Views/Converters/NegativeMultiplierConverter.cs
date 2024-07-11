using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ImageChecker_3.Views.Converters
{
    public class NegativeMultiplierConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var doubles = values.OfType<double>();
            var enumerable = doubles as double[] ?? doubles.ToArray();
            if (enumerable.Length != 0)
            {
                return enumerable.Aggregate((now, next) => now * next) * -1;
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}