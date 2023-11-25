using Domain.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Modals
{
    public class ItemImages : BaseEntityClass
    {
        [Required(ErrorMessage = "Please Select Item...!")]
        public Guid ItemId { get; set; }
        public string Item_Image { get; set;}

        [JsonIgnore]
        public virtual Items Items { get; set; }
    }
}