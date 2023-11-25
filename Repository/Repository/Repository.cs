using Domain.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Repository.ContextClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntityClass
    {
        #region Private Variables
        private readonly ApplicationDbContextClass _DbContext;
        private readonly DbSet<T> _entity;
        #endregion

        #region Constructor
        public Repository(ApplicationDbContextClass Context)
        {
            _DbContext = Context;
            _entity = _DbContext.Set<T>();
        }
        #endregion

        #region GetAll Method
        public async Task<ICollection<T>> GetAll()
        {
            return await _entity.ToListAsync();
        }
        #endregion

        #region GetById Method
        public async Task<T> GetById(Guid id)
        {
            return await _entity.FindAsync(id);
        }
        #endregion

        #region GetLast Method

        public T GetLast()
        {
            if (_entity.ToList() != null)
            {
                return _entity.ToList().LastOrDefault();
            }
            else
            {
                return _entity.ToList().LastOrDefault();
            }


        }
        /* public  T GetLast()
         {
             return _entity.LastOrDefault();
         }*/
        #endregion

        #region Insert Method
        public async Task<bool> Insert(T entity)
        {
            
            await _entity.AddAsync(entity);
            var result = await _DbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
            
        }
        #endregion

        #region Update Method
        public async Task<bool> Update(T entity)
        {
            _entity.Update(entity);
            var result = await _DbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Delete Method
        public async Task<bool> Delete(T entity)
        {

            _entity.Remove(entity);
            var result = await _DbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Find Method
        public async Task<T> Find(Expression<Func<T, bool>> match)
        {
            return await _entity.FirstOrDefaultAsync(match);
        }
        #endregion

        #region FindAll Method
        public async Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match)
        {
            return await _entity.Where(match).ToListAsync();
        }
        #endregion
    }
}
