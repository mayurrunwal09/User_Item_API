using Domain.Modals;
using Domain.ViewModel;
using Repository.Common;
using Repository.Repository;
using Repository.Services.Custom.UserTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.SupplierService
{
    public class SupplierServices : ISupplierServices
    {
        #region Private
        private readonly IRepository<User> _user;
        private readonly IUserTypeServices _userType;
        #endregion

        #region Constructor
        public SupplierServices(IRepository<User> user, IUserTypeServices userType)
        {
            _user = user;
            _userType = userType;
        }
        #endregion

        #region GetAll Method
        public async Task<ICollection<UserViewModel>> GetAll()
        {
            var userType = await _userType.Find(x=> x.TypeName.ToLower().Trim() == "supplier");
            ICollection<UserViewModel> supplierViewModel = new List<UserViewModel>();

            ICollection<User> result = await _user.FindAll(x=>x.UserTypeId == userType.Id);

            foreach(User supplier in result)
            {
                UserViewModel suppliewView = new()
                { 
                    Id = supplier.Id,
                    UserID = supplier.UserID,
                    UserName = supplier.User_Name,
                    UserEmail = supplier.User_Email,
                    UserPassword = Encryptor.DecryptString(supplier.User_Password),
                    UserAddress = supplier.User_Address,
                    UserPhoneno = supplier.User_Phone,
                    UserPhoto = supplier.User_Photo
                };
                UserTypeViewModel supplierTypeView = new();
                if(userType != null)
                {
                    supplierTypeView.Id = userType.Id;
                    supplierTypeView.TypeName = userType.TypeName;
                    suppliewView.UserType.Add(supplierTypeView);
                };
                supplierViewModel.Add(suppliewView);
            }
            if(result == null)
            {
                return null;
            }
            return supplierViewModel;
        }
        #endregion

        #region GetById Method
        public async Task<UserViewModel> GetById(Guid id)
        {
            var result = await _user.GetById(id);
            var userType = await _userType.Find(x => x.TypeName.ToLower().Trim() == "supplier");

            if(userType == null)
            {
                return null;
            }
            else
            {
                if(result.UserTypeId == userType.Id)
                {
                    UserViewModel userView = new()
                    {
                        Id = result.Id,
                        UserID = result.UserID,
                        UserName = result.User_Name,
                        UserEmail = result.User_Email,
                        UserPassword = Encryptor.DecryptString(result.User_Password),
                        UserAddress = result.User_Address,
                        UserPhoneno = result.User_Phone,
                        UserPhoto = result.User_Photo
                    };
                    UserTypeViewModel view = new();
                    if (result != null)
                    {
                        view.Id = userType.Id;
                        view.TypeName = userType.TypeName;
                        userView.UserType.Add(view);
                    }
                    return userView;

                }
                return null;
            }
        }
        #endregion

        #region GetLast Method
        public User GetLast()
        {
            return _user.GetLast();
        }
        #endregion

        #region Insert Method
        public async Task<bool> Insert(UserInsertModel model, string photo)
        {
            var userType = await _userType.Find(x=>x.TypeName.ToLower().Trim() == "supplier");

            if (userType != null)
            {
                User supplier = new User()
                {
                    UserID = model.UserID,
                    User_Name = model.UserName,
                    User_Email = model.Email,
                    User_Password = Encryptor.EncryptString(model.Password),
                    User_Address = model.UserAddress,
                    User_Phone = model.Phoneno,
                    UserTypeId = userType.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = model.IsActive,
                    User_Photo = photo
                };
                var result = await _user.Insert(supplier);
                return result;
            }
            else
                return false;
        }
        #endregion

        #region Update Method
        public async Task<bool> Update(UserUpdateModel model, string photo)
        {
            User supplier = await _user.GetById(model.Id);
            if(supplier != null)
            {
                supplier.UserID = model.UserID;
                supplier.User_Name = model.UserName;
                supplier.User_Email = model.Email;
                supplier.User_Password = Encryptor.EncryptString(model.Password);
                supplier.User_Address = model.UserAddress;
                supplier.User_Phone = model.Phoneno;
                supplier.UserTypeId = supplier.UserTypeId;
                supplier.CreatedOn = supplier.CreatedOn;
                supplier.UpdatedOn = DateTime.Now;
                supplier.IsActive = model.IsActive;
                if(photo == " ")
                {
                    supplier.User_Photo = supplier.User_Photo;
                }
                else
                {
                    supplier.User_Photo = photo;
                }
                var result = await _user.Update(supplier);
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
            if(id !=  Guid.Empty)
            {
                var result = await _user.GetById(id);
                if (result != null)
                {
                    _ = await _user.Delete(result);
                    return true;
                }
                else
                    return false;
            }
            else
            { 
                return false; 
            }
        }
        #endregion

        #region Find Method
        public Task<User> Find(Expression<Func<User, bool>> match)
        {
            return _user.Find(match);
        }
        #endregion        
    }
}
