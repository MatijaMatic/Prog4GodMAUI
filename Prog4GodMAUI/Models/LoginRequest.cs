using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Prog4GodMAUI.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
