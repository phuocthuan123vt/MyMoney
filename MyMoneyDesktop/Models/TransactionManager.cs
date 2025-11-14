using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyMoneyDesktop.Models
{
    public class TransactionManager
    {
        private static TransactionManager? _instance;
        public ObservableCollection<Transaction> Transactions { get; private set; }

        private TransactionManager()
        {
            Transactions = new ObservableCollection<Transaction>();
        }

        // Singleton pattern
        public static TransactionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TransactionManager();
                }
                return _instance;
            }
        }

        // Thêm giao dịch mới
        public void AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                transaction.Id = Transactions.Count > 0 ? Transactions.Max(t => t.Id) + 1 : 1;
                Transactions.Add(transaction);
            }
        }

        // Xóa giao dịch
        public void RemoveTransaction(Transaction transaction)
        {
            if (transaction != null && Transactions.Contains(transaction))
            {
                Transactions.Remove(transaction);
            }
        }

        // Cập nhật giao dịch
        public void UpdateTransaction(int id, Transaction updatedTransaction)
        {
            var transaction = Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transaction.Amount = updatedTransaction.Amount;
                transaction.Type = updatedTransaction.Type;
                transaction.Category = updatedTransaction.Category;
                transaction.Description = updatedTransaction.Description;
                transaction.Date = updatedTransaction.Date;
            }
        }

        // Tính tổng thu nhập
        public decimal GetTotalIncome()
        {
            return Transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);
        }

        // Tính tổng chi tiêu
        public decimal GetTotalExpense()
        {
            return Transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);
        }

        // Tính số dư
        public decimal GetBalance()
        {
            return GetTotalIncome() - GetTotalExpense();
        }

        // Lấy giao dịch theo loại
        public ObservableCollection<Transaction> GetTransactionsByType(string type)
        {
            var result = new ObservableCollection<Transaction>(
                Transactions.Where(t => t.Type == type)
            );
            return result;
        }

        // Lấy giao dịch theo danh mục
        public ObservableCollection<Transaction> GetTransactionsByCategory(string category)
        {
            var result = new ObservableCollection<Transaction>(
                Transactions.Where(t => t.Category == category)
            );
            return result;
        }

        // Lấy giao dịch trong khoảng thời gian
        public ObservableCollection<Transaction> GetTransactionsByDateRange(DateTime from, DateTime to)
        {
            var result = new ObservableCollection<Transaction>(
                Transactions.Where(t => t.Date >= from && t.Date <= to)
            );
            return result;
        }

        // Lấy danh sách danh mục
        public ObservableCollection<string> GetCategories()
        {
            var categories = new ObservableCollection<string>(
                Transactions
                    .Select(t => t.Category)
                    .Distinct()
                    .OrderBy(c => c)
            );
            return categories;
        }

        // Xóa tất cả giao dịch (chỉ dùng cho testing)
        public void ClearAll()
        {
            Transactions.Clear();
        }
    }
}
