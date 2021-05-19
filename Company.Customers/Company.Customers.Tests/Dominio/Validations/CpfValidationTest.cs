using Company.Customers.Domain.Validations;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils.Masks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Company.Customers.Tests.Domain.Validations
{
    public class CpfValidationTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Se_CpfNuloOuVazio_Entao_RetorneFalse(string cpf)
        {
            var cpfValidation = new CpfValidation(null);
            Assert.False(cpfValidation.Validar(cpf));
        }

        [Theory]
        [InlineData("0.4.-01")]
        [InlineData(".454.44-02")]
        [InlineData("08.4.454-03")]
        [InlineData("04.455-04")]
        [InlineData("068.454.444-01")]
        [InlineData("04-01")]
        [InlineData("14.454-01")]
        public void Se_CpfMascarado_E_Invalido_Entao_RetorneFalse(string cpf)
        {
            var cpfValidation = new CpfValidation(new CpfMask());
            Assert.False(cpfValidation.Validar(cpf));
        }

        [Theory]
        [InlineData("00000000000")]
        [InlineData("11111111111")]
        [InlineData("22222222222")]
        [InlineData("33333333333")]
        [InlineData("44444444444")]
        [InlineData("55555555555")]
        [InlineData("66666666666")]
        [InlineData("77777777777")]
        [InlineData("88888888888")]
        [InlineData("99999999999")]
        public void Se_CpfValido_Entao_RetorneTrue(string cpf)
        {
            var cpfValidation = new CpfValidation(new CpfMask());
            Assert.True(cpfValidation.Validar(cpf));
        }

        [Theory]
        [InlineData("1111")]
        [InlineData("dasf ")]
        [InlineData("$d3sa d54q6r3 ")]
        [InlineData("           ")]
        public void Se_CpfInvalido_Entao_RetorneTrue(string cpf)
        {
            var cpfValidation = new CpfValidation(new CpfMask());
            Assert.False(cpfValidation.Validar(cpf));
        }
    }
}
