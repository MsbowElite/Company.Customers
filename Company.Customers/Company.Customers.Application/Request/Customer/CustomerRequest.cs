using System.Text.Json.Serialization;

namespace Company.Customers.Application.Request.Customer
{
    public class CustomerRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
    }
}
