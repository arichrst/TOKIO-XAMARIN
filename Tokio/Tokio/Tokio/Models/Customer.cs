using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tokio.Models
{
    public enum CustomerType { Member = 0 , NonMember = 1 }
    public class Customer : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public string Phone { get; set; }
        [JsonProperty("C")]
        public CustomerType Member { get; set; }
        [JsonProperty("D")]
        public DateTime Created { get; set; }
        [JsonProperty("E")]
        public DateTime Expired { get; set; }
        [JsonProperty("F")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public IEnumerable<Transaction> Transactions { get; set; }
        public Customer()
        {
        }
    }
}
