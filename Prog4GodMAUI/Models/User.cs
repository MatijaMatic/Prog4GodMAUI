using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Prog4GodMAUI.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        public string FullName => $"{Name?.FirstName?.ToUpper()[0]}{Name?.FirstName?.ToLower()[1..]} {Name?.LastName?.ToUpper()[0]}{Name?.LastName?.ToLower()[1..]}";

        public string Initials => $"{Name?.FirstName?.ToUpper()[0]}{Name?.LastName?.ToUpper()[0]}";
    }
}
