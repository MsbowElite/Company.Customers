using Company.Customers.Domain.Entities;
using Company.Customers.Domain.Repository.Interfaces;
using Company.Customers.Domain.Services.Interfaces;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Customers.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerValidation _customerValidation;
        private readonly ICustomerWriterRepository _customerWriteRepository;
        private readonly ICustomerQueryRepository _customerQueryRepository;
        private readonly ICpfValidation _cpfValidation;

        public CustomerService(ICustomerValidation customerValidation,
                              ICustomerWriterRepository customerWriteRepository,
                              ICustomerQueryRepository customerQueryRepository,
                              ICpfValidation cpfValidation)
        {
            _customerValidation = customerValidation;
            _customerWriteRepository = customerWriteRepository;
            _customerQueryRepository = customerQueryRepository;
            _cpfValidation = cpfValidation;
        }
        public async Task<IOperation<Customer>> Register(Customer customer)
        {
            var operation = _customerValidation.Validar(customer);
            if (operation is OperationFail<Customer>)
                return operation;
            var operationCustomerExistente = await ValidarCpfExistente(customer);
            if (operationCustomerExistente is OperationFail<Customer>)
                return operationCustomerExistente;

            var customerCadastrado = await _customerWriteRepository.Cadastrar(customer);
            if (customerCadastrado == null)
                return Result.CreateFailure<Customer>("Houve um erro ao cadastrar esse usuario.");

            return Result.CreateSuccess(customerCadastrado);
        }

        public async Task<IOperation<List<Customer>>> GetAll(int pagina)
        {
            if (pagina <= 0)
                return CriarFalhaConsultaGeralCustomer(pagina);

            var customers = await _customerQueryRepository.Consultar(pagina);
            if(customers == null)
                return Result.CreateFailure<List<Customer>>($"Houve um erro ao realizar a consulta paginada para a pagina {pagina}.");

            return Result.CreateSuccess(customers);

        }


        public async Task<IOperation<Customer>> ListByCpf(string cpf)
        {
            var cpfValido = _cpfValidation.Validar(cpf);
            if (cpfValido)
            {
                var customer = await _customerQueryRepository.Consultar(cpf);
                if (customer is object)
                    return Result.CreateSuccess(customer);

                return Result.CreateFailure<Customer>("O cpf informado é não foi encontrado.");
            }
            return Result.CreateFailure<Customer>("O cpf informado é invalido.");
        }



        private async Task<IOperation<Customer>> ValidarCpfExistente(Customer customer)
        {
            var cpfExiste = await _customerQueryRepository.Consultar(customer.Cpf) != null;
            if (cpfExiste)
            {
                return Result.CreateFailure<Customer>("Houve um erro ao cadastrar esse usuario.", new MessageDetail
                {
                    Field = nameof(customer.Cpf).ToLower(),
                    Message = "O cpf informado já existe",
                    Value = customer.Cpf
                });
            }

            return Result.CreateSuccess<Customer>();
        }

        private IOperation<List<Customer>> CriarFalhaConsultaGeralCustomer(in int pagina)
        {
            return Result.CreateFailure<List<Customer>>("Houve um erro ao iniciar a busca.",
                new MessageDetail
                {
                    Field = nameof(pagina),
                    Message = "A pagina não pode ser menor ou igual a 0.",
                    Value = pagina.ToString()
                });
        }
    }
}
