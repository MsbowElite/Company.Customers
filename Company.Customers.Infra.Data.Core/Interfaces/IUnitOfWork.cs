using System.Threading.Tasks;

namespace Company.Customers.Infra.Data.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
