using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Company.Customers.Infra.Data.Core.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> ListByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<bool> CheckAnyByConditionAsync(Expression<Func<T, bool>> expression);
        void BaseAdd(T entity);
        void BaseUpdate(T entity);
        void BaseRemove(T entity);
    }
}
