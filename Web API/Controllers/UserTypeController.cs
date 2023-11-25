using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Custom.UserTypeService;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeServices _userTypeServices;

        public UserTypeController(IUserTypeServices userTypeServices)
        {
            _userTypeServices = userTypeServices;
        }
        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<UserTypeViewModel>> GetAll()
        {
            var result = await _userTypeServices.GetAllUsers();

            if (result == null)
            {
                return BadRequest("UserType data was Not Found");
            }
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<UserTypeViewModel>> GetById(Guid id)
        {
            var result = await _userTypeServices.GetUserById(id);
            if (result == null)
            {
                return BadRequest("UserType Data Was not Found");
            }
            return Ok(result);
        }

        [HttpPost(nameof(InsertUser))]
        public async Task<IActionResult> InsertUser(UserTypeInsertModel userTypeInsertModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userTypeServices.Insert(userTypeInsertModel);
                if (result == true)
                {
                    return Ok("data Inserted Successfully.....!");
                }
                else
                {
                    return BadRequest("Something Went Wrong.....!");
                }
            }
            else
            {
                return BadRequest("Model State Is not valid...!");
            }
        }

        [Route("UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserTypeUpdateModel userTypeUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userTypeServices.Update(userTypeUpdateModel);
                if (result == true)
                {
                    return Ok("Data Updated Successfully ...... !");
                }
                else
                {
                    return BadRequest("Something went Wrong...... !");
                }
            }
            else
            {
                return BadRequest("ModelState is Not valid....!");
            }
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _userTypeServices.Delete(id);
                if (result == true)
                {
                    return Ok("Data Deleted Successfully....!");
                }
                else
                {
                    return BadRequest("Somthing Went Wrong......!");
                }
            }
            else
            {
                return BadRequest("Id was not Found");
            }
        }
    }
}
