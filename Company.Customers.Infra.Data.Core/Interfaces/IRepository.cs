using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Customers.Infra.Data.Core.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
