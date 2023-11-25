using Domain.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Modals
{
    public class UserType : BaseEntityClass
    {

        [Required(ErrorMessage = "User Type is required...!")]
        [StringLength(10)]
        public string TypeName { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}