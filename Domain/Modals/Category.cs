using Domain.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Modals
{
    public class Category : BaseEntityClass
    {
        [Required(ErrorMessage = "Please Enter Category Name...!")]
        [StringLength(50)]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Items> Items { get; set; }
    }
}