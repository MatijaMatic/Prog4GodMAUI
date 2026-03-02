using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Prog4GodMAUI.Models
{
    public class Rating
    {
        [JsonPropertyName("rate")]
        public double Rate { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
