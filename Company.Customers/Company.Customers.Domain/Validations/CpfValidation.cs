using Company.Customers.Domain.Constants;
using Company.Customers.Domain.Validations.Interfaces;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;

namespace Company.Customers.Domain.Validations
{
    public class CpfValidation : ICpfValidation
    {
		private readonly ICpfMask _cpfMask;
		private static readonly int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        public CpfValidation(ICpfMask cpfMask)
        {
			_cpfMask = cpfMask;

		}
		public bool Validar(string cpf)
        {
			if (string.IsNullOrEmpty(cpf) || string.IsNullOrWhiteSpace(cpf))
				return false;

			cpf = cpf.Trim();
			cpf = _cpfMask.RemoveMaskCpf(cpf);

			if (cpf.Length != CustomerConstants.TAMANHO_CPF)
				return false;

			string cpfTemporario = cpf.Substring(0, 9);

			int soma = 0;
			for (int i = 0; i < 9; i++)
				soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador1[i];

			int resto = soma % 11;
			resto = resto < 2 ? 0 : 11 - resto;

			string digito = resto.ToString();
			cpfTemporario += digito;
			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador2[i];
			resto = soma % 11;
			resto = resto < 2 ? 0 : 11 - resto;
			return cpf.EndsWith(digito + resto.ToString());
		}
    }
}
