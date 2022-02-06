using Manage.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository.Base
{
   public interface IRepository<T> where T : class
    { 
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task UpdateAsync(T entity);
       
        Task DeleteAsync(T entity);

    }
}
