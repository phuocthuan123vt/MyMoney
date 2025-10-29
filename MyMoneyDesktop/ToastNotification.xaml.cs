using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyMoneyDesktop
{
    public partial class ToastNotification : UserControl
    {
        public ToastNotification()
        {
            InitializeComponent();
        }

        public void ShowMessage(string message, string icon = "✔")
        {
            Visibility = Visibility.Visible;
            MessageText.Text = message;
            IconText.Text = icon;

            // Hiệu ứng hiện
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            var slideDown = new DoubleAnimation(-20, 0, TimeSpan.FromMilliseconds(300));
            ToastBorder.BeginAnimation(Border.OpacityProperty, fadeIn);
            (ToastBorder.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.YProperty, slideDown);

            // Hiệu ứng ẩn
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500))
            {
                BeginTime = TimeSpan.FromSeconds(3)
            };
            var slideUp = new DoubleAnimation(0, -20, TimeSpan.FromMilliseconds(500))
            {
                BeginTime = TimeSpan.FromSeconds(3)
            };
            fadeOut.Completed += (s, e) => Visibility = Visibility.Collapsed;

            ToastBorder.BeginAnimation(Border.OpacityProperty, fadeOut);
            (ToastBorder.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.YProperty, slideUp);
        }
    }
}
