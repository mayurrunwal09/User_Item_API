using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Generic
{
    public interface IService<T>
    {
        Task<ICollection<T>> GetAll();
        Task<T> GetById(Guid id);
        T GetLast();
        Task<bool> Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        Task<T> Find(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match);
    }
}
