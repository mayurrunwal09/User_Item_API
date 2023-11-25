using Domain.Modals;
using Domain.ViewModel;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.UserTypeService
{
    public class UserTypeServices : IUserTypeServices
    {
        #region Private Members
        private readonly IRepository<UserType> _userType;
        #endregion

        #region Constructor
        public UserTypeServices(IRepository<UserType> usertype)
        {
            _userType = usertype;
        }
        #endregion

        #region GetAll Methods
        public async Task<ICollection<UserTypeViewModel>> GetAllUsers()
        {
            ICollection<UserTypeViewModel> userTypeViewModels = new List<UserTypeViewModel>();
            ICollection<UserType> userTypes = await _userType.GetAll();

            foreach (UserType userType in userTypes)
            {
                UserTypeViewModel userTypeView = new()
                {
                    Id = userType.Id,
                    TypeName = userType.TypeName
                };
                userTypeViewModels.Add(userTypeView);
            }
            return userTypeViewModels;
        }
        #endregion

        #region GetById Methods
        public async Task<UserTypeViewModel> GetUserById(Guid id)
        {
            var res = await _userType.GetById(id);
            if (res == null)
            {
                return null;
            }
            else
            {
                UserTypeViewModel userTypeViewModel = new()
                {
                    Id = res.Id,
                    TypeName = res.TypeName
                };
                return userTypeViewModel;
            }
        }
        #endregion

        #region GetLast Method
        public UserType GetLast()
        {
            return _userType.GetLast();
        }
        #endregion

        #region Insert Method
        public Task<bool> Insert(UserTypeInsertModel UserTypeInsertModel)
        {
            UserType userType = new()
            {
                TypeName = UserTypeInsertModel.TypeName,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                IsActive = true
            };
            return _userType.Insert(userType);
        }
        #endregion

        #region Update Method
        public async Task<bool> Update(UserTypeUpdateModel UserTypeUpdateModel)
        {
            UserType userType = await _userType.GetById(UserTypeUpdateModel.Id);
            if (userType != null)
            {
                userType.TypeName = UserTypeUpdateModel.TypeName;
                userType.UpdatedOn = DateTime.Now;
                var result = await _userType.Update(userType);
                return result;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Delete Method
        public async Task<bool> Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                UserType userType = await _userType.GetById(id);
                if (userType != null)
                {
                    //Direct Declaration
                    _ = _userType.Delete(userType);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Find Method
        public Task<UserType> Find(Expression<Func<UserType, bool>> match)
        {
            return _userType.Find(match);
        }
        #endregion   
    }
}
