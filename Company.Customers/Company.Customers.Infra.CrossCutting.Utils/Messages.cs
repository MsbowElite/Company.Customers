using System.Collections.Generic;

namespace Company.Customers.Infra.CrossCutting.Utils
{
    public sealed class Messages
    {
        public Messages(in string mensagem, MessageDetail detalhe)
        {
            Campos = new List<MessageDetail>();
            Mensagem = mensagem;
            AdicionarMensagem(detalhe);
        }

        public Messages(in string mensagem, List<MessageDetail> detalhes)
        {
            Mensagem = mensagem;
            Campos = detalhes ?? new List<MessageDetail>();
        }
        public List<MessageDetail> Campos { get; private set; }
        public string Mensagem { get; set; }
        public void AdicionarMensagem(MessageDetail detail)
        {
            if (detail is object)
                Campos.Add(detail);
        }
    }
}
