using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageChecker_3.Views.Converters
{
    public class RoundDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var d = (double)value;
                var isNegative = d < 0;
                d *= 100;
                d = Math.Round(Math.Abs(d)) / 100;
                if (isNegative)
                {
                    d *= -1;
                }

                return d.ToString("F2");
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}