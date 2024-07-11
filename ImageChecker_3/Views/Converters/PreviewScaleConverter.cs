using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageChecker_3.Views.Converters
{
    public class PreviewScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue && parameter is double paramDouble)
            {
                return doubleValue * paramDouble;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}