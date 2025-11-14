using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using MyMoneyDesktop.Models;

namespace MyMoneyDesktop
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private decimal dThuNhap = 0m;
        private decimal dChiTieu = 0m;
        private decimal? dHanMuc;
        private int? iNguongCanhBao;
        private bool toastVisible = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal ThuNhap
        {
            get => dThuNhap;
            set
            {
                if (dThuNhap != value)
                {
                    dThuNhap = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SoDu));
                    OnPropertyChanged(nameof(TiLeTietKiem));
                }
            }
        }

        public decimal ChiTieu
        {
            get => dChiTieu;
            set
            {
                if (dChiTieu != value)
                {
                    dChiTieu = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SoDu));
                    OnPropertyChanged(nameof(TiLeTietKiem));
                }
            }
        }

        public decimal? HanMuc
        {
            get => dHanMuc;
            set
            {
                if (dHanMuc != value)
                {
                    dHanMuc = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? NguongCanhBao
        {
            get => iNguongCanhBao;
            set
            {
                if (iNguongCanhBao != value)
                {
                    iNguongCanhBao = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property tính toán (readonly)
        public decimal SoDu => dThuNhap - dChiTieu;
        public decimal TiLeTietKiem => dThuNhap > 0 ? (SoDu / dThuNhap) * 100m : 0m;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CapNhatThongTin();
            KiemTraVuotHanMuc();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text == "10000000")
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black;
                }
                if (tb.Text == "80")
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus_MoneyLimit(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "10000000";
                tb.Foreground = Brushes.Gray;
            }
        }
        private void TextBox_LostFocus_PercentageLimit(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "80";
                tb.Foreground = Brushes.Gray;
            }
        }

        private void CapNhatThongTin()
        {
            decimal dSoDu = dThuNhap - dChiTieu;
            decimal dTiLeTietKiem = dThuNhap > 0 ? (dSoDu / dThuNhap) * 100m : 0m;

            dHanMuc = Properties.Settings.Default.SpendingLimit;
            iNguongCanhBao = Properties.Settings.Default.WarningThreshold;
            KiemTraVuotHanMuc();
        }

        private void AddTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addTransactionWindow = new AddTransactionWindow
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                
                if (addTransactionWindow.ShowDialog() == true)
                {
                    // Thêm giao dịch vào TransactionManager
                    string type = addTransactionWindow.IsIncome ? "Income" : "Expense";
                    var transaction = new Transaction(
                        addTransactionWindow.Amount,
                        type,
                        "Khác", // Để trống tạm thời, sau có thể cập nhật
                        addTransactionWindow.Description ?? "",
                        addTransactionWindow.TransactionDate
                    );
                    
                    TransactionManager.Instance.AddTransaction(transaction);

                    // Cập nhật dữ liệu tổng quan
                    ThuNhap = TransactionManager.Instance.GetTotalIncome();
                    ChiTieu = TransactionManager.Instance.GetTotalExpense();

                    // Kiểm tra hạn mức sau khi cập nhật
                    KiemTraVuotHanMuc();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm giao dịch: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSaveLimit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sHanMuc = txtMoneyLimit.Text.Trim();
                string sNguong = txtPercentageLimit.Text.Trim();

                if (!decimal.TryParse(sHanMuc, out decimal hanMuc) || hanMuc <= 0)
                {
                    MessageBox.Show("⚠️ Vui lòng nhập hạn mức hợp lệ (số dương).", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(sNguong, out int nguong) || nguong <= 0 || nguong > 100)
                {
                    MessageBox.Show("⚠️ Ngưỡng cảnh báo phải từ 1 đến 100%.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Properties.Settings.Default.SpendingLimit = hanMuc;
                Properties.Settings.Default.WarningThreshold = nguong;
                Properties.Settings.Default.Save();
                dHanMuc = hanMuc;
                iNguongCanhBao = nguong;
                
                // Kiểm tra lại hạn mức ngay sau khi lưu
                KiemTraVuotHanMuc();
                
                // Thông báo thành công
                MessageBox.Show("✅ Cài đặt hạn mức đã được lưu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hạn mức: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void KiemTraVuotHanMuc()
        {
            // Chỉ kiểm tra nếu toggle "Bật hạn mức chi tiêu" được bật
            if (toggleControl == null || !toggleControl.IsChecked)
            {
                HideToast();
                return;
            }

            // Nếu chưa nhập hạn mức (SpendingLimit <= 0), không hiện cảnh báo
            decimal hanMuc = Properties.Settings.Default.SpendingLimit;
            int nguongPercent = Properties.Settings.Default.WarningThreshold;
            
            if (hanMuc <= 0 || nguongPercent <= 0)
            {
                HideToast();
                return;
            }

            decimal nguong = hanMuc * (nguongPercent / 100m);
            
            // Debug: In ra giá trị để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Chi tiêu: {dChiTieu:N0}, Hạn mức: {hanMuc:N0}, Ngưỡng ({nguongPercent}%): {nguong:N0}");

            if (dChiTieu > hanMuc)
            {
                ShowToast("⚠️ Bạn đã vượt quá hạn mức chi tiêu!", new SolidColorBrush(Color.FromArgb(255, 255, 68, 68)));
            }
            else if (dChiTieu >= nguong)
            {
                ShowToast($"⚠️ Chi tiêu đã đạt {nguongPercent}% - Sắp vượt hạn mức chi tiêu!", new SolidColorBrush(Color.FromArgb(255, 255, 193, 7)));
            }
            else
            {
                HideToast();
            }
        }
        private void ShowToast(string message, Brush color)
        {
            ToastText.Text = message;
            ToastNotification.Background = color;

            if (!toastVisible)
            {
                toastVisible = true;
                ToastNotification.Visibility = Visibility.Visible;

                var slide = new DoubleAnimation(-40, 0, TimeSpan.FromSeconds(0.4))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                var fade = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4));

                (ToastNotification.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.YProperty, slide);
                ToastNotification.BeginAnimation(OpacityProperty, fade);
            }
            else
            {
                ToastNotification.Background = color;
                ToastText.Text = message;
            }
        }



        private void HideToast()
        {
            if (toastVisible)
            {
                toastVisible = false;

                var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                fadeOut.Completed += (s, e) =>
                {
                    ToastNotification.Visibility = Visibility.Collapsed;
                };

                ToastNotification.BeginAnimation(OpacityProperty, fadeOut);
            }
        }

        private void ToggleControl_ToggleButtonClicked(object sender, bool isChecked)
        {
            // Khi toggle được bật/tắt, kiểm tra lại hạn mức
            KiemTraVuotHanMuc();
        }
    }
}