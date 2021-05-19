using System;
using System.Text.Json.Serialization;

namespace Company.Customers.Application.Response.Customer
{
    public class CustomerResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string State { get; set; }
        [JsonPropertyName("state")]
        public string Estado { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
    }
}
