using Domain.Modals;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Custom.ItemService
{
    public interface IItemServices
    {
        Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id);
        Task<ItemViewModel> GetById(Guid id);
        Task<bool> Insert(ItemInsertModel itemInsertModel,string photo);
        Task<bool> Update(ItemUpdateModel itemUpdateModel, string photo);
        Task<bool> Delete(Guid id);
        Task<Items> Find(Expression<Func<Items, bool>> match);
        Task<bool> InsertExistingItems(ExistingItemInsertModel itemModel);
    }
}
