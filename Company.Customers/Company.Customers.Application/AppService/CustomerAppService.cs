using Company.Customers.Application.AppService.Interfaces;
using Company.Customers.Application.Mappers;
using Company.Customers.Application.Request.Customer;
using Company.Customers.Application.Response.Customer;
using Company.Customers.Domain.Entities;
using Company.Customers.Domain.Services.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Customers.Application.AppService
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerService _customerService;
        private readonly ICpfMask _cpfMask;
        public CustomerAppService(ICustomerService customerService, ICpfMask cpfMask)
        {
            _customerService = customerService;
            _cpfMask = cpfMask;
        }
        public async Task<IOperation<CustomerResponse>> Register(CustomerRequest request)
        {
            if (request is null)
                return Result.CreateFailure<CustomerResponse>("Requisição vazia");

            request.Cpf = _cpfMask.RemoveMaskCpf(request.Cpf);
            var customer = CustomerRequestMapper.ConverterCustomerRequestEmCustomer(request);
            
            var operation = await _customerService.Register(customer);

            if (operation is OperationFail<Customer> operationFail)
                return Result.CreateFailure<CustomerResponse>(operationFail.Messages.Mensagem, operationFail.Messages.Campos);


            var operationSucess = operation as OperationSuccess<Customer>;
            var customerResponse = CustomerReponseMapper.ConverterCustomerEmCustomerResponse(operationSucess.Data);
            return Result.CreateSuccess(customerResponse);

        }

        public async Task<IOperation<List<CustomerResponse>>> GetAll(int pagina)
        {
            var opereration = await _customerService.GetAll(pagina);
            if (opereration is OperationFail<List<Customer>> operationFail)
                return Result.CreateFailure<List<CustomerResponse>>(operationFail.Messages.Mensagem, operationFail.Messages.Campos);

            var customersOperationSuccess = opereration as OperationSuccess<List<Customer>>;
            var customers =  customersOperationSuccess.Data.Select(x => CustomerReponseMapper.ConverterCustomerEmCustomerResponse(x));
            return Result.CreateSuccess(customers.ToList());

        }

        public async Task<IOperation<CustomerResponse>> ListByCpf(string cpf)
        {
            var operation = await _customerService.ListByCpf(_cpfMask.RemoveMaskCpf(cpf));
            if(operation is OperationFail<Customer> operationFail)
                return Result.CreateFailure<CustomerResponse>(operationFail.Messages.Mensagem, operationFail.Messages.Campos);

            var operationSucess = operation as OperationSuccess<Customer>;
            var customerResponse = CustomerReponseMapper.ConverterCustomerEmCustomerResponse(operationSucess.Data);
            return Result.CreateSuccess(customerResponse);

        }
    }
}
