using Company.Customers.Domain.Entities;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;

namespace Company.Customers.Domain.Validations.Interfaces
{
    public interface ICustomerValidation
    {
        IOperation<Customer> Validar(Customer customer);
    }
}
