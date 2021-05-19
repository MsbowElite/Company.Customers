using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Company.Customers.Infra.CrossCutting.Utils.Interfaces
{
    public interface ICpfMask
    {
        string RemoveMaskCpf([NotNull] in string cpf);
    }
}
