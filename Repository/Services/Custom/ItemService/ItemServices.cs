using Domain.Modals;
using Domain.ViewModel;
using Repository.Repository;
using Repository.Services.Custom.CategoryTypeService;
using Repository.Services.Custom.CustomerService;
using Repository.Services.Custom.SupplierService;
using Repository.Services.Custom.UserTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.ItemService
{
    public class ItemServices : IItemServices
    {
        #region private
        private readonly IRepository<Items> _items;
        private readonly IRepository<User> _user;
        private readonly IRepository<SupplierItems> _supplierItems;
        private readonly IRepository<CustomerItems> _customerItems;
        private readonly IRepository<ItemImages> _itemImages;

        private readonly IUserTypeServices _userTypes;
        private readonly ISupplierServices _suppliers;
        private readonly ICustomerServices _customers;       
        private readonly ICategoryTypeServices _category;
        #endregion

        #region Constructor
        public ItemServices(IRepository<Items> items, IRepository<User> user,IRepository<SupplierItems> supplierItems,IRepository<CustomerItems> customerItems,IRepository<ItemImages> itemImages,IUserTypeServices userTypes,ISupplierServices suppliers,ICustomerServices customers,ICategoryTypeServices category)
        {
            _items = items;
            _user = user;
            _supplierItems = supplierItems;
            _customerItems = customerItems;
            _itemImages = itemImages;
            _userTypes = userTypes;
            _suppliers = suppliers;
            _customers = customers;
            _category = category;
        }
        #endregion

        #region GetAllItemByUser
        public async Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id)
        {
            ICollection<ItemViewModel> itemViewModels = new List<ItemViewModel>();
            var supplier = await _suppliers.Find(x => x.Id == id);
            var customer = await _customers.Find(x => x.Id == id);
            var userType = await _userTypes.Find(x => x.Id == supplier.UserTypeId || x.Id == customer.UserTypeId);

            if(userType.TypeName == "Supplier")
            {
                ICollection<SupplierItems> supplierItems = await _supplierItems.FindAll(x => x.UserId == id);

                if (supplierItems != null)
                {
                    foreach (SupplierItems item in supplierItems)
                    {
                        Items items = await _items.Find(x => x.Id == item.ItemId);
                        ItemViewModel itemView = new()
                        {
                            ItemId = item.ItemId,
                            ItemCode = items.ItemCode,
                            ItemName = items.Item_Name,
                            ItemDescription = items.Item_Description,
                            ItemPrice = items.Item_Price
                        };

                        Category category = await _category.Find(x => x.Id == items.Category_Id);
                        CatogoryViewModel catogoryView = new()
                        {
                            Id = category.Id,
                            CategoryName = category.Name
                        };
                        itemView.Category.Add(catogoryView);

                        User user = await _user.Find(x => x.Id == item.UserId);
                        UserView userView = new()
                        {
                            Id = user.Id,
                            UserName = user.User_Name,
                            UserPhoneno = user.User_Phone,
                            UserEmail = user.User_Email,
                            UserAddress = user.User_Address,
                            UserID = user.UserID,
                            UserPhoto = user.User_Photo
                        };
                        itemView.User.Add(userView);

                        ICollection<ItemImages> itemImages = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                        foreach (var img in itemImages)
                        {
                            ItemImagesView itemImagesView = new()
                            {
                                Id = img.Id,
                                ItemId = img.ItemId,
                                ItemImage = img.Item_Image,
                                IsActive = img.IsActive,
                            };
                            itemView.ItemImages.Add(itemImagesView);
                        }
                        itemViewModels.Add(itemView);
                    }
                    return itemViewModels;
                }
                else
                    return itemViewModels;
            }
            else
            {
                ICollection<CustomerItems> customerItems = await _customerItems.FindAll(x => x.UserId == id);
                foreach (CustomerItems item in customerItems)
                {
                    Items items = await _items.Find(x => x.Id == item.ItemId);
                    ItemViewModel itemView = new()
                    {
                        ItemId = item.ItemId,
                        ItemName = items.Item_Name,
                        ItemCode = items.ItemCode,
                        ItemDescription = items.Item_Description,
                        ItemPrice = items.Item_Price
                    };

                    Category category = await _category.Find(x => x.Id == items.Category_Id);
                    CatogoryViewModel catogoryView = new()
                    {
                        Id = category.Id,
                        CategoryName = category.Name
                    };
                    itemView.Category.Add(catogoryView);

                    User user = await _user.Find(x => x.Id == item.UserId);
                    UserView userView = new()
                    {
                        Id = user.Id,
                        UserName = user.User_Name,
                        UserPhoneno = user.User_Phone,
                        UserEmail = user.User_Email,
                        UserAddress = user.User_Address,
                        UserID = user.UserID,
                        UserPhoto = user.User_Photo
                    };
                    itemView.User.Add(userView);

                    ICollection<ItemImages> itemImages = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                    foreach (var img in itemImages)
                    {
                        ItemImagesView itemImagesView = new()
                        {
                            Id = img.Id,
                            ItemId = img.ItemId,
                            ItemImage = img.Item_Image,
                            IsActive = img.IsActive,
                        };
                        itemView.ItemImages.Add(itemImagesView);
                    }
                    itemViewModels.Add(itemView);
                }
                return itemViewModels;
                
            }
        }
        #endregion

        #region GetById
        public async Task<ItemViewModel> GetById(Guid id)
        {
            ItemViewModel itemViewModel = new();

            SupplierItems supplierItems = await _supplierItems.Find(x => x.ItemId == id);
            if (supplierItems != null)
            {
                Items items = await _items.Find(x => x.Id == supplierItems.ItemId);
                ItemViewModel itemView = new()
                { 
                    ItemId = supplierItems.ItemId,
                    ItemCode = items.ItemCode,
                    ItemName = items.Item_Name,
                    ItemDescription = items.Item_Description,
                    ItemPrice = items.Item_Price
                };

                Category category = await _category.Find(x => x.Id == items.Category_Id);
                CatogoryViewModel catogoryView = new()
                {
                    Id = category.Id,
                    CategoryName = category.Name
                };
                itemView.Category.Add(catogoryView);

                User supplier = await _user.Find(x => x.Id == supplierItems.UserId);
                UserView supplierView = new()
                {
                    Id = supplier.Id,
                    UserName = supplier.User_Name,
                    UserPhoneno = supplier.User_Phone,
                    UserAddress = supplier.User_Address,
                    UserID = supplier.UserID,
                    UserPhoto = supplier.User_Photo
                };
                itemView.User.Add(supplierView);

                ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == supplierItems.ItemId);
                foreach(var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.Item_Image,
                        IsActive = img.IsActive
                    };
                    itemView.ItemImages.Add(imgView);
                }
                return itemView;
            }
            else
            {
                CustomerItems customerItems = await _customerItems.Find(x => x.ItemId == id);
                
                Items items = await _items.Find(x => x.Id == customerItems.ItemId);
                ItemViewModel itemView = new()
                {
                    ItemId = customerItems.ItemId,
                    ItemCode = items.ItemCode,
                    ItemName = items.Item_Name,
                    ItemDescription = items.Item_Description,
                    ItemPrice = items.Item_Price,
                    
                };

                Category category = await _category.Find(x => x.Id == items.Category_Id);
                CatogoryViewModel catogoryView = new()
                {
                    Id = category.Id,
                    CategoryName = category.Name
                };
                itemView.Category.Add(catogoryView);

                User customer = await _user.Find(x => x.Id == customerItems.UserId);
                UserView customerView = new()
                { 
                    Id = customer.Id,
                    UserName = customer.User_Name,
                    UserPhoneno = customer.User_Phone,
                    UserAddress = customer.User_Address,
                    UserID = customer.UserID,
                    UserPhoto = customer.User_Photo
                };
                itemView.User.Add(customerView);

                ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == customerItems.ItemId);
                foreach (var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.Item_Image,
                        IsActive = img.IsActive
                    };
                    itemView.ItemImages.Add(imgView);
                }
                return itemView;
            }
        }
        #endregion

        #region Insert
        public async Task<bool> Insert(ItemInsertModel itemInsertModel, string photo)
        {
            var user = await _user.Find(x => x.Id == itemInsertModel.UserId);
            var userType = await _userTypes.Find(x => x.Id == user.UserTypeId);

            if(userType.TypeName == "Supplier")
            {
                Items item = new()
                {
                    ItemCode = itemInsertModel.ItemCode,
                    Item_Name = itemInsertModel.ItemName,
                    Item_Description = itemInsertModel.ItemDescription,
                    Item_Price = itemInsertModel.ItemPrice,
                    Category_Id = itemInsertModel.CategoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _items.Insert(item);

                if(result == true)
                {
                    SupplierItems supplier = new()
                    { 
                        ItemId = item.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };

                    ItemImages itemImages = new()
                    { 
                        ItemId = item.Id,
                        Item_Image = photo,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultImages = await _itemImages.Insert(itemImages);

                    if (resultImages == true)
                    {
                        await _supplierItems.Insert(supplier);
                        return true;
                    }
                    else 
                        return false;

                }
                else
                    return false;
            }
            else
            {
                Items Items = new()
                { 
                    ItemCode = itemInsertModel.ItemCode,
                    Item_Name = itemInsertModel.ItemName,
                    Item_Description = itemInsertModel.ItemDescription,
                    Item_Price = itemInsertModel.ItemPrice,
                    Category_Id = itemInsertModel.CategoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _items.Insert(Items);

                if(result == true)
                {
                    CustomerItems customerItems = new()
                    { 
                        Id = Items.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };

                    ItemImages itemImages = new()
                    { 
                        ItemId = Items.Id,
                        Item_Image = photo,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultImages = await _itemImages.Insert(itemImages);

                    if (resultImages == true)
                    {
                        await _customerItems.Insert(customerItems);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
        }
        #endregion

        #region InsertExisting
        public async Task<bool> InsertExistingItems(ExistingItemInsertModel itemModel)
        {
            User user = await _user.Find(x => x.Id == itemModel.UserId);
            UserType userType = await _userTypes.Find(x => x.Id == user.UserTypeId);

            if (userType.TypeName == "Supplier")
            {
                SupplierItems supplierItems = new()
                {
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _supplierItems.Insert(supplierItems);
                if (result == true)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                CustomerItems customerItems = new()
                { 
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _customerItems.Insert(customerItems);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        #endregion

        #region Update
        public async Task<bool> Update(ItemUpdateModel itemUpdateModel, string photo)
        {
            Items items = await _items.GetById(itemUpdateModel.Id);

            items.ItemCode = itemUpdateModel.ItemCode;
            items.Item_Name = itemUpdateModel.ItemName;
            items.Item_Description = itemUpdateModel.ItemDescription;
            items.Item_Price = itemUpdateModel.ItemPrice;
            items.CreatedOn = items.CreatedOn;
            items.UpdatedOn = DateTime.Now;
            items.IsActive = itemUpdateModel.IsActive;

            ItemImages itemImages = await _itemImages.Find(x => x.ItemId == itemUpdateModel.Id);
            itemImages.ItemId = items.Id;
            itemImages.CreatedOn = itemImages.CreatedOn;
            itemImages.UpdatedOn = DateTime.Now;
            itemImages.IsActive = itemUpdateModel.IsActive;
            if(photo == null)
            {
                itemImages.Item_Image = itemImages.Item_Image;
            }
            else
            {
                itemImages.Item_Image = photo;
            }
            
            var result = await _items.Update(items);
            if(result == true)
            {
                var resultItemimages = await _itemImages.Update(itemImages);
                if(resultItemimages == true)
                {
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

        #region Delete
        public async Task<bool> Delete(Guid id)
        {
            Items items = await _items.GetById(id);
            if(items != null)
            {
                SupplierItems supplierItems = await _supplierItems.Find(x => x.ItemId == items.Id);
                ItemImages itemImages = await _itemImages.Find(x => x.ItemId == items.Id);
                
                if(supplierItems != null)
                {
                    var resultSupplierItems = await _supplierItems.Delete(supplierItems);
                    if(resultSupplierItems == true) 
                    {
                        var result = await DeleteItemAndItemImages(itemImages, items);
                        return result;
                    }
                    else
                    {
                        var result = await DeleteItemAndItemImages(itemImages, items);
                        return result;
                    }
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

        #region DeleteItemAndItemImages
        private async Task<bool> DeleteItemAndItemImages(ItemImages itemImages, Items items)
        {
            if(itemImages != null)
            {
                var resultItemimages = await _itemImages.Delete(itemImages);
                if(resultItemimages == true)
                {
                    var resultItem = await _items.Delete(items);
                    if(resultItem == true)
                    {
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
            else
            {
                var resultItem = await _items.Delete(items);
                if(resultItem == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Find
        public Task<Items> Find(Expression<Func<Items, bool>> match)
        {
            return _items.Find(match);
        }
        #endregion

        
    }
}
