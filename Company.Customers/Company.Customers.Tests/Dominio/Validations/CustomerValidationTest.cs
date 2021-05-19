using Moq;
using Company.Customers.Domain.Entities;
using Company.Customers.Domain.Validations;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Company.Customers.Tests.Domain.Validations
{
    public class CustomerValidationTest
    {
        [Fact]
        public void Se_CustomerNulo_Entao_RetorneErro()
        {
            var customerValidation = new CustomerValidation(default);
            var operation = customerValidation.Validar(default);
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("A entidade não pode ser vazia.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_NomeDoCustomerNuloOuVazio_Entao_RetorneErro(string nome)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer(nome, "es","cpf"));
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "nome");
            Assert.True(operationFail.Messages.Campos[0].Value == nome);
            Assert.True(operationFail.Messages.Campos[0].Message == "Nome não pode ser vazio.");
        }

        [Fact]
        public void Se_NomeDoCustomerMaiorQue300_Entao_RetorneErro()
        {
            string nome = string.Empty;
            for (; nome.Length < 302;nome += "a");

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer(nome, "es", "cpf"));
            
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "nome");
            Assert.True(operationFail.Messages.Campos[0].Value == nome);
            Assert.True(operationFail.Messages.Campos[0].Message == "Nome não pode ser maior que 300.");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_EstadoDoCustomerNuloOuVazio_Entao_RetorneErro(string estado)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer("nome", estado, "cpf"));
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "estado");
            Assert.True(operationFail.Messages.Campos[0].Value == estado);
            Assert.True(operationFail.Messages.Campos[0].Message == "Estado não pode ser vazio.");
        }

        [Fact]
        public void Se_EstadoDoCustomerMaiorQueDois_Entao_RetorneErro()
        {
            string estado = "123";

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer("nome", estado, "cpf"));
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "estado");
            Assert.True(operationFail.Messages.Campos[0].Value == estado);
            Assert.True(operationFail.Messages.Campos[0].Message == "Estado não pode ser maior que 2.");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_CpfDoCustomerNuloOuVazio_Entao_RetorneErro(string cpf)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer("nome", "es", cpf));
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "cpf");
            Assert.True(operationFail.Messages.Campos[0].Value == cpf);
            Assert.True(operationFail.Messages.Campos[0].Message == "CPF não pode ser vazio.");
        }

        [Fact]
        public void Se_CpfInvalidoCustomerNuloOuVazio_Entao_RetorneErro()
        {
            const string cpf = "123456789";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(false);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer("nome", "es", cpf));
            var operationFail = operation as OperationFail<Customer>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do customer.", operationFail.Messages.Mensagem);
            Assert.True(operationFail.Messages.Campos.Count == 1);
            Assert.True(operationFail.Messages.Campos[0].Field == "cpf");
            Assert.True(operationFail.Messages.Campos[0].Value == cpf);
            Assert.True(operationFail.Messages.Campos[0].Message == "O CPF informado é invalido.");
        }

        [Fact]
        public void Se_CustomerValido_Entao_RetorneSucesso()
        {
            const string cpf = "123456789";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var customerValidation = new CustomerValidation(mockCpfValidation.Object);
            var operation = customerValidation.Validar(new Customer("nome", "es", cpf));
            var operationSucess = operation as OperationSuccess<Customer>;
            Assert.NotNull(operationSucess);
            Assert.Null(operationSucess.Data);
        }
    }
}
