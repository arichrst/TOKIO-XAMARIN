using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tokio.Models
{
    public class Category : BaseModel
    {
        [JsonProperty("A")]
        public string Name { get; set; }
        [JsonProperty("B")]
        public string Code { get; set; }
        [JsonProperty("C")]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public IEnumerable<Product> Products { get; set; }
        public Category()
        {
        }
    }
}
