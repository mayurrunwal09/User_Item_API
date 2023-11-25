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

namespace Repository.Services.Custom.CustomerService
{
    public class CustomerServices : ICustomerServices
    {
        #region Private
        private readonly IRepository<User> _user;
        private readonly IUserTypeServices _userType;
        #endregion

        #region Constructor
        public CustomerServices(IRepository<User> user, IUserTypeServices userType)
        {
            _user = user;
            _userType = userType;
        }
        #endregion

        #region GetAll Method
        public async Task<ICollection<UserViewModel>> GetAll()
        {
            var usertype = await _userType.Find(x => x.TypeName.ToLower().Trim() == "customer");
            ICollection<UserViewModel> customerViewModels = new List<UserViewModel>();

            ICollection<User> result = await _user.FindAll(x => x.UserTypeId == usertype.Id);

            foreach(User customer in  result)
            {
                UserViewModel userViewModel = new()
                {
                    Id = customer.Id,
                    UserID = customer.UserID,
                    UserName = customer.User_Name,
                    UserEmail = customer.User_Email,
                    UserPassword = Encryptor.DecryptString(customer.User_Password),
                    UserAddress = customer.User_Address,
                    UserPhoneno = customer.User_Phone,
                    UserPhoto = customer.User_Photo
                };

                UserTypeViewModel userView = new();
                if(userView != null)
                {
                    userView.Id = usertype.Id;
                    userView.TypeName = usertype.TypeName;
                    userViewModel.UserType.Add(userView);
                }
                customerViewModels.Add(userViewModel);
            }
            if(result == null)
            {
                return null;
            }
            return customerViewModels;
        }
        #endregion

        #region GetById Method
        public async Task<UserViewModel> GetById(Guid id)
        {
            var result = await _user.GetById(id);
            var userType = await _userType.Find(x => x.TypeName.ToLower().Trim() == "customer");

            if(result == null)
            {
                return null;
            }
            else
            {
                if(result.UserTypeId == userType.Id)
                {
                    UserViewModel userview = new()
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
                    if(userType != null)
                    {
                        view.Id = userType.Id;
                        view.TypeName = userType.TypeName;
                        userview.UserType.Add(view);
                    };
                    return userview;
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
        public async Task<bool> Insert(UserInsertModel customerModel, string photo)
        {
            var userType = await _userType.Find(x => x.TypeName.ToLower().Trim() == "customer");
            if (userType != null)
            {
                User customer = new()
                {
                    UserID = customerModel.UserID,
                    User_Name = customerModel.UserName,
                    User_Email = customerModel.Email,
                    User_Password = Encryptor.EncryptString(customerModel.Password),
                    User_Address = customerModel.UserAddress,
                    User_Phone = customerModel.Phoneno,
                    UserTypeId = userType.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = customerModel.IsActive,
                    User_Photo = photo
                };
                var result = await _user.Insert(customer);  
                return result;
            }
            else return false;
        }
        #endregion

        #region Update Method
        public async Task<bool> Update(UserUpdateModel customerUpdatemodel, string photo)
        {
            User customer = await _user.GetById(customerUpdatemodel.Id);
            if (customer != null)
            {
                customer.UserID = customerUpdatemodel.UserID;
                customer.User_Name = customerUpdatemodel.UserName;
                customer.User_Email = customerUpdatemodel.Email;
                customer.User_Password = Encryptor.EncryptString(customerUpdatemodel.Password);
                customer.User_Address = customerUpdatemodel.UserAddress;
                customer.User_Phone = customerUpdatemodel.Phoneno;
                customer.UserTypeId = customer.UserTypeId;
                customer.CreatedOn = customer.CreatedOn;
                customer.UpdatedOn = DateTime.Now;
                customer.IsActive = customerUpdatemodel.IsActive;
                if (photo == " ")
                {
                    customer.User_Photo = customer.User_Photo;
                }
                else
                {
                    customer.User_Photo = photo;
                }
                var result = await _user.Update(customer);
                return result;
            }
            else
                return false;
        }
        #endregion

        #region Delete Method
        public async Task<bool> Delete(Guid id)
        {
            if(id != Guid.Empty)
            {
                User user = await _user.GetById(id);
                if (user != null)
                {
                    _ = await _user.Delete(user);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
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
