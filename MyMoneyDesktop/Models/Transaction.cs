using System;

namespace MyMoneyDesktop.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; } // "Income" hoặc "Expense"
        public string? Category { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public Transaction()
        {
            CreatedAt = DateTime.Now;
            Date = DateTime.Now;
        }

        public Transaction(decimal amount, string type, string category, string description, DateTime date)
        {
            Amount = amount;
            Type = type;
            Category = category;
            Description = description;
            Date = date;
            CreatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Type} - {Amount:N0} ₫ ({Category}) - {Date:dd/MM/yyyy}";
        }
    }
}
