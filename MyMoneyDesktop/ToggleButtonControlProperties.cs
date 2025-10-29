using System.Windows;
using System.Windows.Media;

namespace MyMoneyDesktop
{
    public partial class ToggleButtonControl
    {
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                name: "IsChecked",
                propertyType: typeof(bool),
                ownerType: typeof(ToggleButtonControl),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: false));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty OnColorProperty =
            DependencyProperty.Register(
                name: "OnColor",
                propertyType: typeof(Color),
                ownerType: typeof(ToggleButtonControl),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: Colors.Gray));
        public Color OnColor
        {
            get => (Color)GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }
    }
}