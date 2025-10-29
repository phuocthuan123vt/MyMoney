using System.Globalization;
using System.Windows.Data;

namespace MyMoneyDesktop
{
    internal class CircleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currentValue = (double)value;
            return currentValue - 5;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currentValue = (double)value;
            return currentValue + 5;
        }
    }
}
