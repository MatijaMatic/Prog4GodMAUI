using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Prog4GodMAUI.Models
{
    public class Address
    {
        [JsonPropertyName("geolocation")]
        public Geolocation Geolocation { get; set; } 

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; }

        public string CityCapitilized => $"{City?.ToUpper()[0]}{City?.ToLower()[1..]}";//nije mi jasno
    
        public string CityAndZipcode => $"{CityCapitilized}, {Zipcode}";

        public string FullStreet => $"{Street?.ToUpper()[0]}{Street?.ToLower()[1..]} {Number}";

    }
}
