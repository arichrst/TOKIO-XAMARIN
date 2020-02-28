using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tokio.Services;

namespace Tokio.Models
{
    public enum UserType { Admin = 2 , NonAdmin = 0 , Manager = 1}
    public class User : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public string Phone { get; set; }
        [JsonProperty("C")]
        public string ImageUrl { get; set; }
        [JsonProperty("D")]
        public string Username { get; set; }
        [JsonProperty("E")]
        public string Password { get; set; }
        [JsonProperty("F")]
        public UserType Type { get; set; }
        [JsonProperty("G")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public string Thumbnail { get { return ImageUrl.IsEmpty() ? "user.png" : ImageUrl; } }
        [JsonIgnore]
        public IEnumerable<Store> Stores { get; set; }
        [JsonIgnore]
        public IEnumerable<Transaction> Transactions { get; set; }
        [JsonIgnore]
        public IEnumerable<Shift> Shifts { get; set; }
        public User()
        {
            IsActive = true;
        }
    }
}
