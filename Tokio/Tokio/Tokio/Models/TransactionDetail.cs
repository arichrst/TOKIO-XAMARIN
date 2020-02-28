using System;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public class TransactionDetail : BaseModel
    {
        [JsonProperty("A")]
        public double Qty { get; set; }
        [JsonProperty("B")]
        public double Price { get; set; }
        [JsonProperty("C")]
        public double Total { get; set; }
        [JsonProperty("D")]
        public string ProductName { get; set; }
        [JsonProperty("E")]
        public bool IsMemberTransaction { get; set; }
        [JsonProperty("F")]
        public long ProductId { get; set; }
        [JsonProperty("G")]
        public long TransactionId { get; set; }
        [JsonProperty("H")]
        public string ImageUrl { get; set; }
        [JsonProperty("I")]
        public string Denomination { get; set; }

        [JsonIgnore]
        public string PriceFormatted { get { return Price.ToRupiah(); } }
        [JsonIgnore]
        public string TotalFormated { get { return Total.ToRupiah(); } }
        [JsonIgnore]
        public string ItemInfo { get { return "@" + PriceFormatted + " " + Qty.ToString() + " " + Denomination;  } }

        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public Transaction Transaction { get; set; }
        public TransactionDetail()
        {
        }
    }
}
