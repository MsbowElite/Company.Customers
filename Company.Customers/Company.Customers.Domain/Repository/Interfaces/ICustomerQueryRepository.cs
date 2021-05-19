using Company.Customers.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Customers.Domain.Repository.Interfaces
{
    public interface ICustomerQueryRepository
    {
        Task<Customer> Consultar(string cpf);
        Task<List<Customer>> Consultar(int pagina);
    }
}
