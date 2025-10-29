using System.Globalization;
using System.Windows.Data;

namespace MyMoneyDesktop
{
    internal class BoolToHorizontalAlignementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool currentValue = (bool)value;
            return currentValue ? System.Windows.HorizontalAlignment.Right : System.Windows.HorizontalAlignment.Left;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.HorizontalAlignment currentValue = (System.Windows.HorizontalAlignment)value;
            return currentValue == System.Windows.HorizontalAlignment.Left ? false : true;
        }
    }
}
