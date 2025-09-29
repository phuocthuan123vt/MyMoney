using System;
using System.Windows;

namespace MyMoney
{
    public partial class MainWindow : Window
    {
        private decimal ThuNhap = 0;
        private decimal ChiTieu = 0;
        public MainWindow()
        {
            InitializeComponent();
            CapNhatThongTin();
        }

        private void CapNhatThongTin()
        {
            decimal SoDu = ThuNhap - ChiTieu;
            decimal TiLeTietKiem = ThuNhap > 0 ? (SoDu / ThuNhap) * 100 : 0;
            
            txtThuNhap.Text = $"{ThuNhap:N0}₫";
            txtChiTieu.Text = $"{ChiTieu:N0}₫";
            txtSoDu.Text = $"{SoDu:N0}₫";
            txtTiLeTietKiem.Text = $"{TiLeTietKiem:F1}%";
        }
    }
}
