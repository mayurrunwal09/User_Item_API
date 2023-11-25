using Domain.Modals;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.CustomerService
{
    public interface ICustomerServices
    {
        Task<ICollection<UserViewModel>> GetAll();
        Task<UserViewModel> GetById(Guid id);
        User GetLast();
        Task<bool> Insert(UserInsertModel model, string photo);
        Task<bool> Update(UserUpdateModel model, string photo);
        Task<bool> Delete(Guid id);
        Task<User> Find(Expression<Func<User, bool>> match);
    }
}
