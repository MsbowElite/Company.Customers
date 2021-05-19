using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Company.Customers.Infra.CrossCutting.Utils
{
    public sealed class OperationFail<T> : IOperation<T>
    {
        public OperationFail(in string message,MessageDetail details)
        {
            Messages = new Messages(message, details);
        }

        public OperationFail(in string mensagem, List<MessageDetail> detalhes)
        {
            Messages = new Messages(mensagem, detalhes);
        }

        [JsonPropertyName("messages")]
        public Messages Messages { get; private set; }
    }
}