using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyMoneyDesktop
{
    /// <summary>
    /// Interaction logic for ToggleButtonControl.xaml
    /// </summary>
    public partial class ToggleButtonControl : UserControl
    {
        public event EventHandler<bool>? ToggleButtonClicked;
        public ToggleButtonControl() => InitializeComponent();
     
        private void ToggleButton_Click(object sender, MouseButtonEventArgs e)
        {
            IsChecked = !IsChecked;
            background.Background = IsChecked ? new SolidColorBrush(OnColor) : new SolidColorBrush(Colors.Gray);
            ToggleButtonClicked?.Invoke(this, IsChecked);
        }
    }
}
