using Company.Customers.Domain.Entities;
using System.Threading.Tasks;

namespace Company.Customers.Domain.Repository.Interfaces
{
    public interface ICustomerWriterRepository
    {
        Task<Customer> Cadastrar(Customer cartao);
    }
}
