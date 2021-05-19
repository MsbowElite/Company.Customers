using System;

namespace Company.Customers.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Customer(string nome, string estado,string cpf)
        {
            Nome = nome;
            Estado = estado;
            Cpf = cpf;
        }

        public Customer(Guid id, string nome, string estado, string cpf) : this(nome,estado,cpf)
        {
            Id = id;
        }

        public string Nome { get; private set; }
        public string Estado { get; private set; }
        public string Cpf { get; private set; }

    }
}
