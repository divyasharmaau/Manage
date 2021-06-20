using Manage.Core.Entities.Base;
using Manage.Core.Repository.Base;
using Manage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ManageContext _manageContext;


        //public Repository(MContext mContext, ICon)
        public Repository(ManageContext manageContext )
        {
            _manageContext = manageContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _manageContext.Set<T>().Add(entity);
            await _manageContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _manageContext.Set<T>().Remove(entity);
            await _manageContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(T entity)
        {
            _manageContext.Entry(entity).State = EntityState.Modified;
            await _manageContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _manageContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _manageContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _manageContext.Set<T>().FindAsync(id);
            
        }

      

    }
}
