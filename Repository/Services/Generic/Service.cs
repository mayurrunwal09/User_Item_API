using Domain.BaseEntity;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Generic
{
    public class Service<T> : IService<T> where T : BaseEntityClass
    {
        #region Private
        private readonly IRepository<T> _repository;
        #endregion

        #region Constructor
        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }
        #endregion

        #region GetAll Method
        public Task<ICollection<T>> GetAll()
        {
            return _repository.GetAll();
        }
        #endregion

        #region GetById Method
        public Task<T> GetById(Guid id)
        {
            return _repository.GetById(id);
        }
        #endregion

        #region GetLast Method
        public T GetLast()
        {
            return _repository.GetLast();
        }
        #endregion

        #region Insert Method
        public Task<bool> Insert(T entity)
        {
            return _repository.Insert(entity);
        }
        #endregion

        #region Update Method
        public Task<bool> Update(T entity)
        {
            return _repository.Update(entity);
        }
        #endregion

        #region Delete Method
        public Task<bool> Delete(T entity)
        {
            return _repository.Delete(entity);
        }
        #endregion

        #region Find Method
        public Task<T> Find(Expression<Func<T, bool>> match)
        {
            return _repository.Find(match);
        }
        #endregion

        #region FindAll Method
        public Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match)
        {
            return _repository.FindAll(match);
        }
        #endregion
    }
}
