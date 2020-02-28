using System;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public class Expense : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public double Debit { get; set; }
        [JsonProperty("C")]
        public double Credit { get; set; }
        [JsonProperty("D")]
        public DateTime Created { get; set; }
        [JsonProperty("E")]
        public string BillNumber { get; set; }
        [JsonProperty("G")]
        public string PaymentMethod { get; set; }
        [JsonProperty("H")]
        public long UserId { get; set; }
        [JsonProperty("I")]
        public long StoreId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }

        [JsonIgnore]
        public string DebitFormatted { get { return Debit.ToRupiah(); } }
        [JsonIgnore]
        public string CreditFormatted { get { return Credit.ToRupiah(); } }
        public Expense()
        {
        }
    }
}
