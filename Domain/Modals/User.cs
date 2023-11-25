using Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Modals
{
    public class User : BaseEntityClass
    {
        [Required(ErrorMessage = "Please Enter UserId...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "UserID start with # and Only Number and character are allowed eg(#User1001)")]
        [StringLength(10)]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please Enter UserName...!")]
        [StringLength(100)]
        public string User_Name { get; set; }

        [RegularExpression(@"/\S+@\S+\.\S/", ErrorMessage = "Enter Valid Email...!")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "Please Enter Address...!")]
        [StringLength(500)]
        public string User_Address { get; set; }

        [RegularExpression(@"/^[+]?(\d{1,2})+\-(\d{10})/", ErrorMessage = "Enter Valid Phone No eg(+91-1234578596)...!")]
        public string User_Phone { get; set; }

        public string User_Photo { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Password { get; set; }

        [Required(ErrorMessage = "Please Select UserType...!")]
        public Guid UserTypeId { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }
        public virtual ICollection<CustomerItems> CustomerItems { get; set; }
        public virtual ICollection<SupplierItems> SupplierItems { get; set; }
        
    }
}
