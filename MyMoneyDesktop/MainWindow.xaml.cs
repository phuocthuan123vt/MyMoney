using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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

    public MainWindow()
    {
        InitializeComponent();
        CapNhatThongTin();
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
}