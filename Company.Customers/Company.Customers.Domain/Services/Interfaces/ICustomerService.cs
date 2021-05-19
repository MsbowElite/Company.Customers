using Company.Customers.Domain.Entities;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Customers.Domain.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IOperation<Customer>> Register(Customer customer);
        Task<IOperation<Customer>> ListByCpf(string cpf);
        Task<IOperation<List<Customer>>> GetAll(int pagina);
    }
}
