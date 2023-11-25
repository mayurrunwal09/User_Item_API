using Domain.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Modals
{
    public class Items : BaseEntityClass
    {
        [Required(ErrorMessage = "Please Enter ItemCode...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "Item Code start with # and Only Number and character are allowed eg(#Item1001)")]
        [StringLength(10)]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Please Enter Item Name...!")]
        [StringLength(100)]
        public string Item_Name { get; set; }

        [Required(ErrorMessage = "Please Enter Item Description...!")]
        public string Item_Description { get; set;}

        [Required(ErrorMessage = "Please Enter Item Price...!")]
        public string Item_Price { get; set; }

        [Required(ErrorMessage = "Please select Category...!")]
        public Guid Category_Id { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
        public virtual CustomerItems CustomerItems { get; set; }
        public virtual SupplierItems SuppliersItems { get; set; }
        public virtual ICollection<ItemImages> ItemImages { get; set; }
    }
}