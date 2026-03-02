using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Prog4GodMAUI.Models
{
    public class Name
    {
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
    }
}
