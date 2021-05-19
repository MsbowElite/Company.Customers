using System.Text.Json.Serialization;

namespace Company.Customers.Infra.CrossCutting.Utils
{
    public sealed class MessageDetail
    {
        [JsonPropertyName("field")]
        public string Field { get;  set; }
        [JsonPropertyName("message")]
        public string Message { get;  set; }
        [JsonPropertyName("value")]
        public string Value { get;  set; }
    }
}
