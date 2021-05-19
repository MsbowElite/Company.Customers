using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Company.Customers.Infra.CrossCutting.Utils.Masks
{
    public sealed class CpfMask : ICpfMask
    {
        public string RemoveMaskCpf([NotNull] in string cpf)
        {
            return cpf.Replace(".", string.Empty).Replace("-", string.Empty);
        }
    }
}
