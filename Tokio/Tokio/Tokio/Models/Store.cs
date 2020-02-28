using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public class Store : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public string Address { get; set; }
        [JsonProperty("C")]
        public DateTime Created { get; set; }
        [JsonProperty("D")]
        public DateTime Expired { get; set; }
        [JsonProperty("E")]
        public string ImageUrl { get; set; }
        [JsonProperty("F")]
        public bool IsActive { get; set; }
        [JsonProperty("G")]
        public double Tax { get; set; }
        [JsonProperty("H")]
        public double ServiceCharge { get; set; }
        [JsonProperty("I")]
        public string ReceiptMessage { get; set; }
        [JsonProperty("J")]
        public string OVO { get; set; }
        [JsonProperty("K")]
        public string GoPay { get; set; }
        [JsonProperty("L")]
        public string Dana { get; set; }
        [JsonProperty("M")]
        public string LinkAja { get; set; }
        [JsonProperty("N")]
        public string Instagram { get; set; }
        [JsonProperty("O")]
        public string Twitter { get; set; }
        [JsonProperty("P")]
        public string Facebook { get; set; }
        [JsonProperty("Q")]
        public long UserId { get; set; }
        [JsonProperty("R")]
        public string BankAccount { get; set; }
        [JsonProperty("S")]
        public string BankName { get; set; }
        [JsonProperty("T")]
        public string BankAccountName { get; set; }

        [JsonIgnore]
        public string Icon { get { return ImageUrl.IsEmpty() ? "store.png" : ImageUrl; } }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public IEnumerable<Transaction> Transactions { get; set; }
        [JsonIgnore]
        public IEnumerable<Expense> Expenses { get; set; }
        public Store()
        {
            Created = DateTime.Now;
            Expired = Created.AddYears(1);
            IsActive = true;
        }

        public void Deactive()
        {
            IsActive = false;
        }

        public void Active()
        {
            IsActive = true;
        }
    }
}
