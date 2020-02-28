using System;
using Tokio.Models;

namespace Tokio.Services
{
    public class Database
    {
        public DataStore<User> Users { get; set; }
        public DataStore<Transaction> Transactions { get; set; }
        public DataStore<TransactionDetail> TransactionDetails { get; set; }
        public DataStore<Product> Products { get; set; }
        public DataStore<Category> Categories { get; set; }
        public DataStore<Expense> Expenses { get; set; }
        public DataStore<Customer> Customers { get; set; }
        public DataStore<Ledger> Ledgers { get; set; }
        public DataStore<Store> Stores { get; set; }
        public DataStore<Shift> Shifts { get; set; }
        public Database()
        {
            Users = new DataStore<User>();
            Transactions = new DataStore<Transaction>();
            TransactionDetails = new DataStore<TransactionDetail>();
            Products = new DataStore<Product>();
            Categories = new DataStore<Category>();
            Expenses = new DataStore<Expense>();
            Customers = new DataStore<Customer>();
            Ledgers = new DataStore<Ledger>();
            Stores = new DataStore<Store>();
            Shifts = new DataStore<Shift>();
        }
    }
}
