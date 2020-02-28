using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public class Transaction : BaseModel
    {
        [JsonProperty("A")]
        public string Code { get; set; }
        [JsonProperty("B")]
        public DateTime Created { get; set; }
        [JsonProperty("D")]
        public double Discount { get; set; }
        [JsonProperty("E")]
        public double Total { get; set; }
        [JsonProperty("F")]
        public double Tax { get; set; }
        [JsonProperty("G")]
        public double ServiceCharge { get; set; }
        [JsonProperty("H")]
        public double GrandTotal { get; set; }
        [JsonProperty("I")]
        public double PaymentAmount { get; set; }
        [JsonProperty("J")]
        public double Return { get; set; }
        [JsonProperty("K")]
        public string PaymentMethod { get; set; }
        [JsonProperty("L")]
        public bool IsMemberTransaction { get; set; }
        [JsonProperty("M")]
        public long UserId { get; set; }
        [JsonProperty("N")]
        public long CustomerId { get; set; }
        [JsonProperty("O")]
        public long StoreId { get; set; }
        [JsonProperty("P")]
        public DateTime PaidDate { get; set; }
        [JsonProperty("Q")]
        public bool IsPaid { get; set; }

        [JsonProperty("X")]
        public bool IsReceived { get; set; }
        [JsonProperty("Y")]
        public bool IsOnProgress { get; set; }
        [JsonProperty("Z")]
        public bool IsFinished { get; set; }

        [JsonIgnore]
        public string DiscountFormatted { get { return Discount.ToRupiah(); } }
        [JsonIgnore]
        public string TotalFormatted { get { return Total.ToRupiah(); } }
        [JsonIgnore]
        public string TaxFormatted { get { return Tax.ToRupiah(); } }
        [JsonIgnore]
        public string ServiceChargeFormatted { get { return ServiceCharge.ToRupiah(); } }
        [JsonIgnore]
        public string GrandTotalFormatted { get { return GrandTotal.ToRupiah(); } }
        [JsonIgnore]
        public string PaymentAmountFormatted { get { return PaymentAmount.ToRupiah(); } }
        [JsonIgnore]
        public string ReturnFormatted { get { return Return.ToRupiah(); } }

        [JsonIgnore]
        public Store Store { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
        [JsonIgnore]
        public ObservableCollection<TransactionDetail> TransactionDetails { get; set; }

        public Transaction()
        {
            Code = DateTime.Now.Ticks.ToString();
        }

        public Expense AsExpense()
        {
            return new Expense()
            {
                BillNumber = Code,
                Created = Created,
                Credit = GrandTotal,
                Debit = 0,
                Name = "TRANSACTION " + Code,
                StoreId = StoreId,
                UserId = UserId
            };
        }


        public async Task ScanOnProgress(Database db)
        {
            this.IsReceived = true;
            this.IsOnProgress = true;
            await db.Transactions.UpdateItemAsync(this);
        }

        public async Task ScanFinished(Database db)
        {
            this.IsReceived = true;
            this.IsOnProgress = true;
            this.IsFinished = true;
            await db.Transactions.UpdateItemAsync(this);
        }

        public async Task ScanPaid(Database db)
        {
            this.IsPaid = true;
            await db.Transactions.UpdateItemAsync(this);
        }
    }
}
