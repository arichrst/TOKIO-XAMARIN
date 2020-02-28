using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tokio.Services;
using Xamarin.Forms;

namespace Tokio.Models
{
    public class Product : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public string Code { get; set; }
        [JsonProperty("C")]
        public string ImageUrl { get; set; }
        [JsonProperty("D")]
        public double Price { get; set; }
        [JsonProperty("E")]
        public double MemberPrice { get; set; }
        [JsonProperty("F")]
        public string Denomination { get; set; }
        [JsonProperty("G")]
        public string Description { get; set; }
        [JsonProperty("H")]
        public bool IsActive { get; set; }
        [JsonProperty("I")]
        public long CategoryId { get; set; }

        [JsonIgnore]
        public string PriceFormatted { get { return Price.ToRupiah(); } }
        [JsonIgnore]
        public string MemberPriceFormatted { get { return MemberPrice.ToRupiah(); } }

        [JsonIgnore]
        public Category Category { get; set; }
        [JsonIgnore]
        public IEnumerable<TransactionDetail> TransactionDetails { get; set; }

        [JsonIgnore]
        public double WidthView { get { return Device.Idiom == TargetIdiom.Tablet ? App.ScreenWidth / 6 : App.ScreenWidth / 3; } }
        [JsonIgnore]
        public double HeightView { get { return Device.Idiom == TargetIdiom.Tablet ? (App.ScreenWidth / 6) + 30 : (App.ScreenWidth / 3) + 30; } }

        public Product()
        {
            
        }

    }
}
