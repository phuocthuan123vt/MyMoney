﻿using System;
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

namespace MyMoneyDesktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Nên đặt readonly nếu là dữ liệu mẫu, hoặc chuyển sang property khi cần binding
    private decimal dThuNhap = 15_000_000m;
    private decimal dChiTieu = 5_000_000m;
    private decimal? dHanMuc;
    private int? iNguongCanhBao;
    private bool toastVisible = false;

    public MainWindow()
    {
        InitializeComponent();
        CapNhatThongTin();
        KiemTraVuotHanMuc();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var tb = sender as TextBox;
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

    private void TextBox_LostFocus_MoneyLimit(object sender, RoutedEventArgs e)
    {
        var tb = sender as TextBox;
        if (string.IsNullOrWhiteSpace(tb.Text))
        {
            tb.Text = "10000000";
            tb.Foreground = Brushes.Gray;
        }
    }
    private void TextBox_LostFocus_PercentageLimit(object sender, RoutedEventArgs e)
    {
        var tb = sender as TextBox;
        if (string.IsNullOrWhiteSpace(tb.Text))
        {
            tb.Text = "80";
            tb.Foreground = Brushes.Gray;
        }
    }

    private void CapNhatThongTin()
    {
        decimal dSoDu = dThuNhap - dChiTieu;
        decimal dTiLeTietKiem = dThuNhap > 0 ? (dSoDu / dThuNhap) * 100m : 0m;

        txtThuNhap.Text = $"{dThuNhap:N0} ₫";
        txtChiTieu.Text = $"{dChiTieu:N0} ₫";
        txtSoDu.Text = $"{dSoDu:N0} ₫";
        txtTiLeTietKiem.Text = $"{dTiLeTietKiem:F1}%";
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
            addTransactionWindow.ShowDialog();
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
            KiemTraVuotHanMuc();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khi lưu hạn mức: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void KiemTraVuotHanMuc()
    {
        if (dHanMuc == null || iNguongCanhBao == null) return;

        decimal hanMuc = dHanMuc.Value;
        decimal nguong = hanMuc * (iNguongCanhBao.Value / 100m);

        if (dChiTieu >= hanMuc)
        {
            ShowToast("Bạn đã vượt hạn mức chi tiêu!", new SolidColorBrush(Colors.Red));
        }
        else if (dChiTieu >= nguong)
        {
            ShowToast($"Chi tiêu đã đạt hơn {iNguongCanhBao}% hạn mức!", new SolidColorBrush(Colors.Gold));
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
}