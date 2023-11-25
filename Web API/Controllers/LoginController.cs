using Domain.Helper;
using Domain.Modals;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Common;
using Repository.Services.Custom.CustomerService;
using Repository.Services.Custom.SupplierService;
using Repository.Services.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Web_API.Middleware.Auth;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Private variables and constructor
        private readonly ILogger _logger;
        private readonly ICustomerServices _customerServices;
        private readonly ISupplierServices _supplierServices;
        private readonly IJWTAuthManager _authManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IService<UserType> _userType;

        public LoginController(ILogger<LoginController> logger, ICustomerServices customerServices, ISupplierServices supplierServices, IJWTAuthManager authManager, IWebHostEnvironment webHostEnvironment, IService<UserType> userType)
        {
            _logger = logger;
            _customerServices = customerServices;
            _supplierServices = supplierServices;
            _authManager = authManager;
            _webHostEnvironment = webHostEnvironment;
            _userType = userType;
        }
        #endregion

        #region Login
        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin(LoginModel loginModel)
        {
            Response<string> response = new();

            if(ModelState.IsValid)
            {
                var user = await _supplierServices.Find(x => x.User_Name == loginModel.UserName && x.User_Password == Encryptor.EncryptString(loginModel.Password));
                if(user == null)
                {
                    response.Message = "Invalid UserName / Password";
                    response.Status = (int)HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                else
                {
                    response.Message = _authManager.GenerateJWT(user);
                    response.Status = (int)HttpStatusCode.OK;
                    return Ok(response);
                }
            }
            else
            {
                response.Message = "Invalid Login Information";
                response.Status = (int)HttpStatusCode.NotAcceptable;
                return BadRequest(response);
            }
        }
        #endregion

        #region Supplier registration
        [HttpPost(nameof(RegisterSupplier))]
        public async Task<IActionResult> RegisterSupplier([FromForm] UserInsertModel supplierModel)
        {
            if(ModelState.IsValid)
            {
                var userType = await _userType.Find(x => x.TypeName.ToLower().Trim() == "supplier");
                if(userType != null)
                {
                    if(supplierModel.ProfilePhoto != null)
                    {
                        var ChechUser = await _supplierServices.Find(x => x.UserID == supplierModel.UserID);
                        if(ChechUser != null)
                        {
                            return BadRequest($"User Id : {supplierModel.UserID} , is Already Exist...!");
                        }
                        else
                        {
                            var CheckUserName = await _supplierServices.Find(x => x.User_Name.ToLower().Trim() == supplierModel.UserName.ToLower().Trim());
                            if(CheckUserName != null)
                            {
                                return BadRequest($"User Name : {supplierModel.UserName} , is Already Exist...!");
                            }
                        }

                        var photo = await UploadPhoto(supplierModel.ProfilePhoto,supplierModel.UserName, DateTime.Now.ToString("dd/mm/yyyy"));
                        if(string.IsNullOrEmpty(photo))
                        {
                            return BadRequest("Error While Uploading Supplier photo , Please Try Again.... !");
                        }
                        
                        var result = await _supplierServices.Insert(supplierModel, photo);
                        if(result == true)
                        {
                            return Ok("Supplier Register Successfully..... !");
                        }
                        else
                        {
                            return BadRequest("Error While Resgistering Supplier .... !");
                        }
                        
                    }
                    else
                    {
                        return BadRequest("Please Upload Phofile photo...!");
                    }
                }
                else
                {
                    return BadRequest("Something Went Wrong , Please try Again later .... !");
                }
            }
            else
            {
                return BadRequest("Invalid supplier Information , please enter valid credential...! ");
            }
        }
        #endregion

        #region Customer Registration
        [HttpPost(nameof(RegisterCustomer))]
        public async Task<IActionResult> RegisterCustomer([FromForm] UserInsertModel customerModel)
        {
            if(ModelState.IsValid)
            {
                var userType = await _userType.Find(x => x.TypeName.ToLower().Trim() == "customer");
                if(userType != null)
                {
                    if (customerModel.ProfilePhoto != null)
                    {
                        var checkUser = await _customerServices.Find(x => x.UserID == customerModel.UserID);
                        if(checkUser != null)
                        {
                            return BadRequest($"User Id : {customerModel.UserID} , is Already Exist...!");
                        }
                        else
                        {
                            var checkUserName = await _customerServices.Find(x => x.User_Name.ToLower().Trim() == customerModel.UserName.ToLower().Trim());
                            if(checkUserName != null)
                            {
                                return BadRequest($"User Id : {customerModel.UserName} , is Already Exist...!");
                            }
                        }

                        var photo = await UploadPhoto(customerModel.ProfilePhoto,customerModel.UserName,DateTime.Now.ToString("dd/mm/yyyy"));
                        if(string.IsNullOrEmpty(photo))
                        {
                            return BadRequest("Error While Uploading Supplier photo , Please Try Again.... !");
                        }

                        var result = await _customerServices.Insert(customerModel , photo);
                        if(result == true)
                        {
                            return Ok("Customer Register Successfully ...!");
                        }
                        else
                        {
                            return BadRequest("Error While Registering customer...!");
                        }
                        
                        
                    }
                    else
                    {
                        _logger.LogWarning("Error : Please Upload Profile Photo");
                        return BadRequest("Please Upload Profile Photo....!");
                    }
                }
                else
                {
                    return BadRequest("Something Went Wrong, Please try again later...!");
                }
            }
            else
            {
                _logger.LogWarning("Error : Invalid Customer Information..... !");
                return BadRequest("Invalid customer Information , please enter valid credential...!");
            }
        }
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
