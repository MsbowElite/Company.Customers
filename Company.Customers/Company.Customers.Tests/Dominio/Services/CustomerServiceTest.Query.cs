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
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Se_PaginaDeConsultaPaginadaMenorOuIgualAhZero_Entao_RetorneErro(int pagina)
        {
            var customerService = new CustomerService(null, null, null, null);
            var operation = await customerService.GetAll(pagina);
            var operationFail = operation as OperationFail<List<Customer>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao iniciar a busca.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.Equal(nameof(pagina), operationFail.Messages.Campos[0].Field);
            Assert.Equal("A pagina não pode ser menor ou igual a 0.", operationFail.Messages.Campos[0].Message);
            Assert.Equal(pagina.ToString(), operationFail.Messages.Campos[0].Value);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public async Task Se_ConsultaPaginadaRetornarUmaListaNula_Entao_RetoneErro(int pagina)
        {
            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(pagina))
                          .Returns(Task.FromResult(default(List<Customer>)));

            var customerService = new CustomerService(null, null, mockCustomerQueyRepository.Object, null);

            var operation = await customerService.GetAll(pagina);
            var operationFail = operation as OperationFail<List<Customer>>;
            Assert.NotNull(operationFail);
            Assert.Equal($"Houve um erro ao realizar a consulta paginada para a pagina {pagina}.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_ConsultaPaginadaRetornarUmaListaNaoNula_Entao_RetoneSuceso()
        {
            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(1))
                          .Returns(Task.FromResult(new List<Customer>()));

            var customerService = new CustomerService(null, null, mockCustomerQueyRepository.Object, null);

            var operation = await customerService.GetAll(1);
            var operationSucess = operation as OperationSuccess<List<Customer>>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfForInvalido_Entao_RetoneErro()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(false);
            var customerService = new CustomerService(null, null, null, mockCpfValidation.Object);

            var operation = await customerService.ListByCpf(It.IsAny<string>());
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal($"O cpf informado é invalido.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfNaoForEncontrado_Entao_RetoneErro()
        {
            const string cpf = "12345678911";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Customer)));

            var customerService = new CustomerService(null, null, mockCustomerQueyRepository.Object, mockCpfValidation.Object);

            var operation = await customerService.ListByCpf(cpf);
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal($"O cpf informado é não foi encontrado.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfForEncontrado_Entao_RetoneSucesso()
        {
            const string cpf = "12345678911";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var mockCustomerQueyRepository = new Mock<ICustomerQueryRepository>();
            mockCustomerQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(new Customer("","","")));

            var customerService = new CustomerService(null, null, mockCustomerQueyRepository.Object, mockCpfValidation.Object);

            var operation = await customerService.ListByCpf(cpf);
            var operationSucess = operation as OperationSuccess<Customer>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
