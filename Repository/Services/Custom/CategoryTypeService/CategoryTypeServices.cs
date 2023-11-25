using Domain.Modals;
using Domain.ViewModel;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.CategoryTypeService
{
    public class CategoryTypeServices : ICategoryTypeServices
    {
        #region private
        private readonly IRepository<Category> _category;
        #endregion

        #region Constructor
        public CategoryTypeServices(IRepository<Category> category)
        {
            _category = category;
        }
        #endregion


        #region GetAll Method
        public async Task<ICollection<CatogoryViewModel>> GetAllCategory()
        {
            ICollection<CatogoryViewModel> catogoryViewModels = new List<CatogoryViewModel>();
            ICollection<Category> categories = await _category.GetAll();

            foreach(Category category in categories)
            {
                CatogoryViewModel catogoryViewModel = new()
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                };
                catogoryViewModels.Add(catogoryViewModel);
            }
            return catogoryViewModels;
        }
        #endregion

        #region GetById Method
        public async Task<CatogoryViewModel> GetCategoryById(Guid id)
        {
            var result = await _category.GetById(id);
            if(result == null)
            {
                return null;
            }
            else
            {
                CatogoryViewModel catogoryViewModel = new()
                {
                    Id = result.Id,
                    CategoryName = result.Name,
                };
                return catogoryViewModel; 
            }
        }
        #endregion

        #region GetLast Method
        public Category GetLast()
        {
            return _category.GetLast();
        }
        #endregion

        #region Insert Method
        public Task<bool> InsertCategory(CategoryInsertModel CategoryInsertModel)
        {
            Category category = new()
            {
                Name = CategoryInsertModel.CategoryName,
            };
            return _category.Insert(category);
        }
        #endregion

        #region Update method
        public async Task<bool> UpdateCategory(CategoryUpdateModel CategoryUpdateModel)
        {
            Category category = await _category.GetById(CategoryUpdateModel.Id);
            if(category != null)
            {
                category.Name = CategoryUpdateModel.CategoryName;
                category.UpdatedOn = DateTime.Now;
                var result = await _category.Update(category);
                return result;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Delete method
        public async Task<bool> DeleteCategory(Guid id)
        {
            if(id != Guid.Empty)
            {
                var result = await _category.GetById(id);
                if(result != null)
                {
                    _ = _category.Delete(result);
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

        #region Find method
        public Task<Category> Find(Expression<Func<Category, bool>> match)
        {
            return _category.Find(match);
        }
        #endregion  
    }
}
