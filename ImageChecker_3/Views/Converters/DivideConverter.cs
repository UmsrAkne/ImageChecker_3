using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageChecker_3.Views.Converters
{
    public class DivideConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] is not double a || values[1] is not double b)
            {
                return Binding.DoNothing;
            }

            if (b != 0)
            {
                return a / b;
            }
            else
            {
                throw new DivideByZeroException("The denominator (b) cannot be zero.");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}