using System.Windows;

namespace MyMoneyDesktop
{
    /// <summary>
    /// Interaction logic for AddTransactionWindow.xaml
    /// </summary>
    public partial class AddTransactionWindow : Window
    {
        public decimal Amount { get; set; }
        public bool IsIncome { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }

        public AddTransactionWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            dpDate.SelectedDate = DateTime.Now;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            // Validate amount
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("⚠️ Vui lòng nhập số tiền hợp lệ (số dương).", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get values
            Amount = amount;
            IsIncome = rbIncome.IsChecked ?? false;
            Description = txtDescription.Text;
            TransactionDate = dpDate.SelectedDate ?? DateTime.Now;

            // Return data to parent
            this.DialogResult = true;
            this.Close();
        }
    }
}
