using Moq;
using Company.Customers.Domain.Entities;
using Company.Customers.Domain.Repository.Interfaces;
using Company.Customers.Domain.Services;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Company.Customers.Tests.Domain.Services
{
    public partial class CustomerServiceTest
    {
        [Fact]
        public async Task Se_CustomerNulo_Entao_RetorneErro()
        {

            var mockValidation = new Mock<ICustomerValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateFailure<Customer>("A entidade não pode ser vazia."));

            var customerService = new CustomerService(mockValidation.Object,null,null,null);
            var operation =  await customerService.Register(null);
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("A entidade não pode ser vazia.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfCustomerExistir_Entao_RetorneErro()
        {
            const string cpf = "1111111111";
            var customer = new Customer(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<ICustomerValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Customer)));

            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(customer));

            var customerService = new CustomerService(mockValidation.Object, null, 
                                                    mockCustomerQueyRepository.Object,null);

            var operation =  await customerService.Register(customer);
            var operationFail = operation as OperationFail<Customer>;
            Assert.Equal("Houve um erro ao cadastrar esse usuario.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.Equal("cpf",operationFail.Messages.Campos[0].Field);
            Assert.Equal("O cpf informado já existe", operationFail.Messages.Campos[0].Message);
            Assert.Equal(cpf, operationFail.Messages.Campos[0].Value);

        }

        [Fact]
        public async Task Se_RespositorioNaoCadastrarCustomer_Entao_RetorneErro()
        {
            const string cpf = "1111111111";
            var customer = new Customer(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<ICustomerValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Customer)));

            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Customer)));

            var mockCustomerWriterRepository = new Mock<ICustomerWriterRepository>();
            mockCustomerWriterRepository.Setup(x => x.Cadastrar(customer))
                          .Returns(Task.FromResult(default(Customer)));

            var customerService = new CustomerService(mockValidation.Object, mockCustomerWriterRepository.Object,
                                                    mockCustomerQueyRepository.Object, null);

            var operation = await customerService.Register(customer);
            var operationFail = operation as OperationFail<Customer>;
            Assert.Equal("Houve um erro ao cadastrar esse usuario.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);

        }


        [Fact]
        public async Task Se_ValidacoesCorretas_Entao_CadastrarCustomer()
        {
            const string cpf = "1111111111";
            var customer = new Customer(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<ICustomerValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Customer)));

            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Customer)));

            var mockCustomerWriterRepository = new Mock<ICustomerWriterRepository>();
            mockCustomerWriterRepository.Setup(x => x.Cadastrar(customer))
                          .Returns(Task.FromResult(customer));

            var customerService = new CustomerService(mockValidation.Object, mockCustomerWriterRepository.Object,
                                                    mockCustomerQueyRepository.Object, null);

            var operation = await customerService.Register(customer);
            var operationSucess = operation as OperationSuccess<Customer>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
            Assert.Equal(customer.Cpf, operationSucess.Data.Cpf);
            Assert.Equal(customer.Estado, operationSucess.Data.Estado);
            Assert.Equal(customer.Nome, operationSucess.Data.Nome);
            Assert.Equal(customer.Id, operationSucess.Data.Id);

        }
    }
}
