using System.Globalization;
using System.Windows.Data;

namespace MyMoneyDesktop
{
    internal class HalfOfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currentValue = (double)value;
            return currentValue / 2;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currentValue = (double)value;
            return currentValue * 2;
        }
    }
}
