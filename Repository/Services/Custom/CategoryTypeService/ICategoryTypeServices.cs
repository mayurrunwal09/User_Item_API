using Domain.Modals;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.CategoryTypeService
{
    public interface ICategoryTypeServices
    {
        Task<ICollection<CatogoryViewModel>> GetAllCategory();
        Task<CatogoryViewModel> GetCategoryById(Guid id);
        Category GetLast();
        Task<bool> InsertCategory(CategoryInsertModel CategoryInsertModel);
        Task<bool> UpdateCategory(CategoryUpdateModel CategoryUpdateModel);
        Task<bool> DeleteCategory(Guid id);
        Task<Category> Find(Expression<Func<Category,bool>> match);

    }
}
