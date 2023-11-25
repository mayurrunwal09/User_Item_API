using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Custom.CustomerService;
using Repository.Services.Custom.SupplierService;
using Repository.Services.Custom.UserTypeService;
using System.Text.RegularExpressions;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Private variables and constructor
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICustomerServices _customer;
        private readonly ISupplierServices _supplier;
        private readonly IUserTypeServices _userType;

        public UserController(ILogger<UserController> logger, IWebHostEnvironment webHostEnvironment, ICustomerServices customer, ISupplierServices supplier, IUserTypeServices userType)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _customer = customer;
            _supplier = supplier;
            _userType = userType;
        }
        #endregion


        #region Supplier

        #region GetAllSupplier
        [HttpGet(nameof(GetAllSupplier))]
        public async Task<ActionResult<UserViewModel>> GetAllSupplier()
        {
            var result = await _supplier.GetAll();
            if (result == null)
            {
                return BadRequest("Records not found .. !");
            }
            return Ok(result);
        }
        #endregion

        #region GetSupplierById
        [HttpGet(nameof(GetSupplierById))]
        public async Task<ActionResult<UserViewModel>> GetSupplierById(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _supplier.GetById(id);
                if (result == null)
                {
                    return BadRequest("Records not found .. !");
                }
                return Ok(result);
            }
            else
                return BadRequest("Id is Empty");
        }
        #endregion

        #region UpdateSupplier
        [HttpPut(nameof(UpdateSupplier))]
        public async Task<IActionResult> UpdateSupplier([FromForm] UserUpdateModel supplierModel)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _supplier.Find(x => x.UserID == supplierModel.UserID && x.Id != supplierModel.Id);
                if (checkUser != null)
                {
                    return BadRequest($"User Id :- {supplierModel.UserID} , is Already Exist ....... !");
                }
                else
                {
                    var checkUserName = await _supplier.Find(x => x.User_Name == supplierModel.UserName && x.Id != supplierModel.Id);
                    if (checkUser != null)
                    {
                        return BadRequest($"User Name :- {supplierModel.UserName} , is Already Exist ....... !");
                    }
                }

                if (supplierModel.ProfilePhoto != null)
                {
                    var photo = await UploadPhoto(supplierModel.ProfilePhoto, supplierModel.UserName, DateTime.Now.ToString("dd/mm/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                    {
                        return BadRequest("Error while uploading supplier photo, please try again ... !");
                    }
                    var result = await _supplier.Update(supplierModel, photo);
                    if (result == true)
                    {
                        return Ok("Suppplier Updated Successfully ....!");

                    }
                    else
                        return BadRequest("Something went wrong Supplier is not updated successfully ...!");

                }
                else
                {
                    var result = await _supplier.Update(supplierModel, " ");
                    if (result == true)
                    {
                        return Ok("Suppplier Updated Successfully ....!");
                    }
                    else
                        return BadRequest("Something went wrong Supplier is not updated successfully ...!");
                }
            }
            else
            {
                return NotFound("Supplier Not Found with id :" + supplierModel.Id + ", Please Try Again After Sometime...!");
            }
        }
        #endregion

        #region  DeleteSupplier
        [HttpDelete(nameof(DeleteSupplier))]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var result = await _supplier.Delete(id);
            if (result == true)
            {
                return Ok("Supplier Recoed Deleted Successefully ... !");
            }
            else
                return BadRequest("Something went wrong Supplier is not deleted.....!");
        }
        #endregion

        #endregion

        #region Customer 

        #region GetAllCustomer
        [HttpGet(nameof(GetAllCustomer))]
        public async Task<ActionResult<UserViewModel>> GetAllCustomer()
        {
            var result = await _customer.GetAll();
            if (result == null)
            {
                return BadRequest("Records not found .. !");
            }
            return Ok(result);
        }
        #endregion

        #region GetCustomerById
        [HttpGet(nameof(GetCustomerById))]
        public async Task<ActionResult<UserViewModel>> GetCustomerById(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _customer.GetById(id);
                if (result == null)
                {
                    return BadRequest("Records not found .. !");
                }
                return Ok(result);
            }
            else
                return BadRequest("Id is Empty");
        }
        #endregion

        #region UpdateCustomer
        [HttpPut(nameof(UpdateCustomer))]
        public async Task<IActionResult> UpdateCustomer([FromForm] UserUpdateModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _customer.Find(x => x.UserID == customerModel.UserID && x.Id != customerModel.Id);
                if (checkUser != null)
                {
                    return BadRequest($"User Id :- {customerModel.UserID} , is Already Exist ....... !");
                }
                else
                {
                    var checkUserName = await _customer.Find(x => x.User_Name == customerModel.UserName && x.Id != customerModel.Id);
                    if (checkUser != null)
                    {
                        return BadRequest($"User Name :- {customerModel.UserName} , is Already Exist ....... !");
                    }
                }

                if (customerModel.ProfilePhoto != null)
                {
                    var photo = await UploadPhoto(customerModel.ProfilePhoto, customerModel.UserName, DateTime.Now.ToString("dd/mm/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                    {
                        return BadRequest("Error while uploading Customer photo, please try again ... !");
                    }
                    var result = await _customer.Update(customerModel, photo);
                    if (result == true)
                    {
                        return Ok("Customer Updated Successfully ....!");

                    }
                    else
                        return BadRequest("Something went wrong Customer is not updated successfully ...!");

                }
                else
                {
                    var result = await _customer.Update(customerModel, " ");
                    if (result == true)
                    {
                        return Ok("Suppplier Updated Successfully ....!");
                    }
                    else
                        return BadRequest("Something went wrong Customer is not updated successfully ...!");
                }
            }
            else
            {
                return NotFound("Customer Not Found with id :" + customerModel.Id + ", Please Try Again After Sometime...!");
            }
        }
        #endregion

        #region DeleteCustomer
        [HttpDelete(nameof(DeleteCustomer))]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customer.Delete(id);
            if (result == true)
            {
                return Ok("Supplier Recoed Deleted Successefully ... !");
            }
            else
                return BadRequest("Something went wrong Customer is not deleted.....!");
        }
        #endregion

        #endregion

        #region UploadPhoto
        private async Task<string?> UploadPhoto(IFormFile file, string Id, string date)
        {
            string filename;
            string contentPath = this._webHostEnvironment.ContentRootPath;
            var extension = "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                filename = Id.ToLower() + "-" + date + extension;
                string outFileName = Regex.Replace(filename, @"[^0-9a-zA-Z.]+", "");
                var pathbuilde = Path.Combine(contentPath, "Images\\User");

                if (!Directory.Exists(pathbuilde))
                {
                    Directory.CreateDirectory(pathbuilde);
                }
                var path = Path.Combine(contentPath, "Images\\User", outFileName);
                Console.WriteLine(path);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return outFileName;
            }
            else
                return "";

        }
        #endregion
    }
}
