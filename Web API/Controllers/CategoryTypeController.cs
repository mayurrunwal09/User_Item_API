using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Custom.CategoryTypeService;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryTypeController : ControllerBase
    {
        private readonly ICategoryTypeServices _categoryTypeServices;

        public CategoryTypeController(ICategoryTypeServices categoryTypeServices)
        {
            _categoryTypeServices = categoryTypeServices;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<CatogoryViewModel>> GetAll()
        {
            var result = await _categoryTypeServices.GetAllCategory();
            if (result == null)
            {
                return BadRequest("Something Went Wrong ........!");
            }
            return Ok(result);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<ActionResult<CatogoryViewModel>> GetById(Guid id)
        {
            var result = await _categoryTypeServices.GetCategoryById(id);
            if (result == null)
            {
                return BadRequest("Something Went Wrong ........!");
            }
            return Ok(result);
        }

        [Route("InsertCategory")]
        [HttpPost]
        public async Task<IActionResult> InsertCategory(CategoryInsertModel categoryInsertModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryTypeServices.InsertCategory(categoryInsertModel);
                if (result == true)
                {
                    return Ok("Data Inserted Successfully .... !");
                }
                return BadRequest("Something Went Wrong .... !");
            }
            return BadRequest("ModelState is Not Valid .... !");
        }

        [Route("UpdateCategory")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateModel categoryUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryTypeServices.UpdateCategory(categoryUpdateModel);
                if (result == true)
                {
                    return Ok("Data Updated Successfully ....... !");
                }
                else
                {
                    return BadRequest("Something Went Wrong .... !");
                }
            }
            else
            {
                return BadRequest("ModelState is not valid .... !");
            }
        }

        [Route("Deletecategory")]
        [HttpDelete]
        public async Task<IActionResult> Deletecategory(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _categoryTypeServices.DeleteCategory(id);
                if (result == true)
                {
                    return Ok("Data Deleted Successfully ....!");
                }
                else
                {
                    return BadRequest("Something went wrong....!");
                }
            }
            else
            {
                return BadRequest("Id Is Not Found");
            }
        }
    }
}
