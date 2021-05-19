using Company.Customers.Application.Request.Customer;
using Company.Customers.Application.Response.Customer;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Customers.Application.AppService.Interfaces
{
    public interface ICustomerAppService
    {
        Task<IOperation<CustomerResponse>> Register(CustomerRequest request);
        Task<IOperation<CustomerResponse>> ListByCpf(string cpf);
        Task<IOperation<List<CustomerResponse>>> GetAll(int pagina);
    }
}
