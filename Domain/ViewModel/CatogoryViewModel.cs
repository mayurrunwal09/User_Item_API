using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class CatogoryViewModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryInsertModel
    {
        [Required(ErrorMessage = "Please Enter Category Name...!")]
        [StringLength(50, ErrorMessage = "Category name can't exceed to 50 character...!")]
        public string CategoryName { get; set; }
    }

    public class CategoryUpdateModel : CategoryInsertModel
    {
        [Required(ErrorMessage = "Id is Required for Updating Category...!")]
        public Guid Id { get; set; }
    }
}
