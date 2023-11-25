using Domain.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Modals
{
    public class CustomerItems : BaseEntityClass
    {
        [Required(ErrorMessage = "Please Enter User Id...!")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Please Enter Item Id...!")]
        public Guid ItemId { get; set; }

        public virtual User User { get; set; }
        public virtual Items Items { get; set; }
    }
}