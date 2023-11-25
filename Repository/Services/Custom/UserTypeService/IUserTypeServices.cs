using Domain.Modals;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.UserTypeService
{
    public interface IUserTypeServices
    {
        Task<ICollection<UserTypeViewModel>> GetAllUsers();
        Task<UserTypeViewModel> GetUserById(Guid id);
        UserType GetLast();
        Task<bool> Insert(UserTypeInsertModel UserTypeInsertModel);
        Task<bool> Update(UserTypeUpdateModel UserTypeUpdateModel);
        Task<bool> Delete(Guid id);
        Task<UserType> Find(Expression<Func<UserType, bool>> match);

    }
}
