using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public class Shift
    {
        [JsonProperty("A")]
        public DateTime Start { get; set; }
        [JsonProperty("B")]
        public DateTime End { get; set; }
        [JsonProperty("C")]
        public DateTime Created { get; set; }
        [JsonProperty("D")]
        public double Debit { get; set; }
        [JsonProperty("E")]
        public double Credit { get; set; }
        [JsonProperty("F")]
        public string Summary
        {
            get
            {
                var total = Debit - Credit;
                return total <= 0 ? "LOSS" : "PROFIT";
            }
        }
        [JsonProperty("G")]
        public long UserId { get; set; }
        [JsonProperty("H")]
        public string Notes { get; set; }


        [JsonIgnore]
        public string DebitFormatted { get { return Debit.ToRupiah(); } }
        [JsonIgnore]
        public string CreditFormatted { get { return Credit.ToRupiah(); } }
        public Shift()
        {
            Created = DateTime.Now;
        }

        public async Task<Shift> LetsWrite(DateTime start, DateTime end)
        {
            var expenses = await new List<Expense>().FromDB(x => x.Created.BetweenDate(start, end));
            var transactions = await new List<Transaction>().FromDB(x => x.PaidDate.BetweenDate(start, end) && x.IsPaid);
            var result = ShowMeDetails(expenses, transactions);
            this.Start = start;
            this.End = end;
            this.Debit = result.Sum(x => x.Debit);
            this.Credit = result.Sum(x => x.Credit);
            return await Task.FromResult(this);
        }

        private IEnumerable<Expense> ShowMeDetails(IEnumerable<Expense> expenses, IEnumerable<Transaction> transactions)
        {
            if (expenses == null || transactions == null)
                return new List<Expense>();
            List<Expense> temporary = new List<Expense>();
            foreach (var item in expenses)
            {
                temporary.Add(item);
            }
            foreach (var item in transactions)
            {
                temporary.Add(item.AsExpense());
            }
            return temporary.OrderBy(x => x.Created).ToList();
        }
    }
}
