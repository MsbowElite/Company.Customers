using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Text.Json.Serialization;

namespace Company.Customers.Infra.CrossCutting.Utils
{
    public sealed class OperationSuccess<T> : IOperation<T>
    {
        public OperationSuccess(T data)
        {
            Data = data;
        }
        [JsonPropertyName("data")]
        public T Data { get; }
    }
}