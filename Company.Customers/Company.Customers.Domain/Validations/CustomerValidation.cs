using Company.Customers.Domain.Constants;
using Company.Customers.Domain.Entities;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Company.Customers.Domain.Validations
{
    public class CustomerValidation : ICustomerValidation
    {
        private readonly ICpfValidation _cpfValidation;

        public CustomerValidation(ICpfValidation cpfValidation)
        {
            _cpfValidation = cpfValidation;
        }

        public IOperation<Customer> Validar(Customer customer)
        {
            if (customer is null)
                return Result.CreateFailure<Customer>("A entidade não pode ser vazia.");

            var erros = new List<MessageDetail>();

            ValidaDadosVazios(customer, erros);
            ValidarCpf(customer.Cpf, erros);

            if (erros.Any())
                return Result.CreateFailure<Customer>("Houve um erro ao validar os dados do customer.", erros);

            return Result.CreateSuccess<Customer>();

        }

        private void ValidarCpf(in string cpf, List<MessageDetail> erros)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                erros.Add(new MessageDetail
                {
                    Field = nameof(cpf),
                    Value = cpf,
                    Message = "CPF não pode ser vazio."
                });
            }
            else
            {
                if (!_cpfValidation.Validar(cpf))
                {
                    erros.Add(new MessageDetail
                    {
                        Field = nameof(cpf),
                        Value = cpf,
                        Message = "O CPF informado é invalido."

                    });
                }
            }
        }

        private void ValidaDadosVazios(Customer customer, List<MessageDetail> erros)
        {
            if (string.IsNullOrEmpty(customer.Nome))
            {
                erros.Add(new MessageDetail
                {
                    Field = nameof(customer.Nome).ToLower(),
                    Value = customer.Nome,
                    Message = "Nome não pode ser vazio."

                });
            }
            else
            {
                if (customer.Nome.Length > CustomerConstants.TAMANHO_MAX_NOME)
                {
                    erros.Add(new MessageDetail
                    {
                        Field = nameof(customer.Nome).ToLower(),
                        Value = customer.Nome ,
                        Message = $"Nome não pode ser maior que {CustomerConstants.TAMANHO_MAX_NOME}."

                    });
                }
            }

            if (string.IsNullOrEmpty(customer.Estado))
            {
                erros.Add(new MessageDetail
                {
                    Field = nameof(customer.Estado).ToLower(),
                    Value = customer.Estado,
                    Message = "Estado não pode ser vazio."
                });
            }
            else
            {
                if (customer.Estado.Length > CustomerConstants.TAMANHO_MAX_ESTADO)
                {
                    erros.Add(new MessageDetail
                    {
                        Field = nameof(customer.Estado).ToLower(),
                        Value = customer.Estado,
                        Message = $"Estado não pode ser maior que {CustomerConstants.TAMANHO_MAX_ESTADO}."

                    });
                }
            }

        }
    }
}
