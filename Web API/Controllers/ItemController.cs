using Domain.Modals;
using Domain.ViewModel;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Custom.CategoryTypeService;
using Repository.Services.Custom.ItemService;
using Repository.Services.Custom.SupplierService;
using System.Text.RegularExpressions;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        #region Private variable and Constructor
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IItemServices _itemService;
        private readonly ISupplierServices _userServices;
        private readonly ICategoryTypeServices _categoryTypeService;

        public ItemController(ILogger<ItemController> logger, IWebHostEnvironment environment, IItemServices itemService, ISupplierServices supplierServices, ICategoryTypeServices categoryTypeService)
        {
            _logger = logger;
            _environment = environment;
            _itemService = itemService;
            _userServices = supplierServices;
            _categoryTypeService = categoryTypeService;
        }
        #endregion

        #region Methods

        #region GetAllItemsByUsers
        [HttpGet(nameof(GetAllItemsBySupplier))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemsBySupplier(Guid id)
        {
            ICollection<ItemViewModel> items = await _itemService.GetAllItemByUser(id);
            if (items == null)
            {
                return BadRequest("No record are found ...!");
            }
            return Ok(items);
        }

        [HttpGet(nameof(GetAllItemsByCustomer))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemsByCustomer(Guid id)
        {
            ICollection<ItemViewModel> items = await _itemService.GetAllItemByUser(id);
            if (items == null)
            {
                return BadRequest("No Record are Found ... !");
            }
            return Ok(items);
        }
        #endregion

        #region GetItem

        [HttpGet(nameof(GetItem))]
        public async Task<ActionResult<ItemViewModel>> GetItem(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _itemService.GetById(id);
                if (result == null)
                {
                    return BadRequest("No Items Are Found.....!");
                }
                return Ok(result);
            }
            else
                return BadRequest("Invalid ModelState, please enter valid information .... !");
        }
        #endregion

        #region AddExistingItemToUsers

        [HttpPost(nameof(AddExistingItemToSupplier))]
        public async Task<IActionResult> AddExistingItemToSupplier([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _userServices.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var checkItem = await _itemService.Find(x => x.Id == itemModel.Id);
                    if (checkItem == null)
                    {
                        return BadRequest($"Item not Exist which have id {itemModel.Id} , please provide valid id or try after some time ...!");
                    }
                    else
                    {
                        var checkUser = await _userServices.Find(x => x.Id == itemModel.UserId);
                        if (checkUser != null)
                        {
                            _logger.LogInformation("Starting Insertion of items in the supplier .. !");
                            var result = await _itemService.InsertExistingItems(itemModel);
                            if (result == true)
                            {
                                return Ok("Item Inserted Successfully ...... !");
                            }
                            else
                                return BadRequest("Somthing went wrong, Invalid supplier information ...!");
                        }
                        else
                        {
                            _logger.LogWarning($"User is not exist whith the id {itemModel.UserId} , plese provide valid user id");
                            return BadRequest($"User is not exist whith the id {itemModel.UserId} , plese provide valid user id");
                        }
                    }
                }
                else
                {
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
                }
            }
            else
            {
                return BadRequest("Supplier information is not valid, please enter valid information ... !");
            }
        }

        [HttpPost(nameof(AddExistingItemToCustomer))]
        public async Task<IActionResult> AddExistingItemToCustomer([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User Customer = await _userServices.Find(x => x.Id == itemModel.UserId);
                if (Customer != null)
                {
                    var checkItem = await _itemService.Find(x => x.Id == itemModel.Id);
                    if (checkItem == null)
                    {
                        return BadRequest($"Item not Exist which have id {itemModel.Id} , please provide valid id or try after some time ...!");
                    }
                    else
                    {
                        var checkUser = await _userServices.Find(x => x.Id == itemModel.UserId);
                        if (checkUser != null)
                        {
                            _logger.LogInformation("Starting Insertion of items in the Customer .. !");
                            var result = await _itemService.InsertExistingItems(itemModel);
                            if (result == true)
                            {
                                return Ok("Item Inserted Successfully ...... !");
                            }
                            else
                                return BadRequest("Somthing went wrong, Invalid Customer information ...!");
                        }
                        else
                        {
                            _logger.LogWarning($"User is not exist whith the id {itemModel.UserId} , plese provide valid user id");
                            return BadRequest($"User is not exist whith the id {itemModel.UserId} , plese provide valid user id");
                        }
                    }
                }
                else
                {
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
                }
            }
            else
            {
                return BadRequest("Customer information is not valid, please enter valid information ... !");
            }
        }
        #endregion

        #region AddUsersItem

        [HttpPost(nameof(AddSupplierItems))]
        public async Task<IActionResult> AddSupplierItems([FromForm] ItemInsertModel itemInsert)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _userServices.Find(x => x.Id == itemInsert.UserId);
                if (supplier != null)
                {
                    var checkItemCode = await _itemService.Find(x => x.ItemCode == itemInsert.ItemCode);
                    if (checkItemCode != null)
                    {
                        return BadRequest($"Item Is already exist with the code {itemInsert.ItemCode} , please try after some time");
                    }
                    else
                    {
                        var checkname = await _itemService.Find(x => x.Item_Name == itemInsert.ItemName);
                        if (checkname != null)
                        {
                            return BadRequest($"Item Is already exist with the name {itemInsert.ItemName} , please try after some time");
                        }
                    }
                    var category = await _categoryTypeService.GetCategoryById(itemInsert.CategoryId);
                    if (category == null)
                    {
                        return BadRequest("Category is not found , please provide valid category");

                    }
                    var photo = await UploadPhoto(itemInsert.ItemImage, itemInsert.ItemName);
                    var result = await _itemService.Insert(itemInsert, photo);
                    if (result == true)
                    {
                        return Ok("Item Inserted Successfully ... !");
                    }
                    else
                        return BadRequest("Something went wrong , plase try ageain ..!");
                }
                else
                {
                    return BadRequest("Unauthorized supplier ... !");
                }
            }
            else
            {
                _logger.LogWarning("Invalid Supplier informtion , please provalid valid information .... !");
                return BadRequest("Invalid Supplier Information , please provide valid information .... !");
            }

        }

        [HttpPost(nameof(AddCustomerItems))]
        public async Task<IActionResult> AddCustomerItems([FromForm] ItemInsertModel itemInsert)
        {
            if (ModelState.IsValid)
            {
                User customer = await _userServices.Find(x => x.Id == itemInsert.UserId);
                if (customer != null)
                {
                    var checkItemCode = await _itemService.Find(x => x.ItemCode == itemInsert.ItemCode);
                    if (checkItemCode != null)
                    {
                        return BadRequest($"Item Is already exist with the code {itemInsert.ItemCode} , please try after some time");
                    }
                    else
                    {
                        var checkname = await _itemService.Find(x => x.Item_Name == itemInsert.ItemName);
                        if (checkname != null)
                        {
                            return BadRequest($"Item Is already exist with the name {itemInsert.ItemName} , please try after some time");
                        }
                    }
                    var category = await _categoryTypeService.GetCategoryById(itemInsert.CategoryId);
                    if (category == null)
                    {
                        return BadRequest("Category is not found , please provide valid category");

                    }
                    var photo = await UploadPhoto(itemInsert.ItemImage, itemInsert.ItemName);
                    var result = await _itemService.Insert(itemInsert, photo);
                    if (result == true)
                    {
                        return Ok("Item Inserted Successfully ... !");
                    }
                    else
                        return BadRequest("Something went wrong , plase try ageain ..!");
                }
                else
                {
                    return BadRequest("Unauthorized customer ... !");
                }
            }
            else
            {
                _logger.LogWarning("Invalid customer informtion , please provalid valid information .... !");
                return BadRequest("Invalid customer Information , please provide valid information .... !");
            }
        }
        #endregion

        #region EditUsers

        [HttpPut(nameof(UpdateItems))]
        public async Task<IActionResult> UpdateItems([FromForm] ItemUpdateModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Finding user for update which have id " + itemModel.Id);
                ItemViewModel itemView = await _itemService.GetById(itemModel.Id);
                if (itemView.ItemId == itemModel.Id)
                {
                    if (itemView != null)
                    {
                        var checkItem = await _itemService.Find(x => x.ItemCode == itemModel.ItemCode && x.Id != itemModel.Id);
                        if (checkItem != null)
                        {
                            return BadRequest($"Item Code {itemModel.ItemCode} , is already exist .... !");
                        }
                        else
                        {
                            var checkName = await _itemService.Find(x => x.Item_Name == itemModel.ItemName);
                            if (checkName != null)
                            {
                                return BadRequest($"Item Name {itemModel.ItemName} , is already exist .... !");
                            }
                        }

                        if (itemModel.ItemImage == null)
                        {
                            var result = await _itemService.Update(itemModel, null);
                            if (result == true)
                            {
                                return Ok("User Updated Successfully ... !");
                            }
                            else
                            {
                                return BadRequest("Something went wrong while updating the user");
                            }
                        }
                        else
                        {
                            var photo = await UploadPhoto(itemModel.ItemImage, itemModel.ItemName);
                            var result = await _itemService.Update(itemModel, photo);
                            if (result == true)
                            {
                                return Ok("Item Updated Successfully ... !");
                            }
                            else
                            {
                                return BadRequest("Something went wrong while updating the user");
                            }
                        }

                    }
                    else
                    {
                        return NotFound($"Item Not Found. id : {itemModel.Id}, Please Provide Valid Details and Try Again...!");
                    }
                }
                else
                {
                    return BadRequest("Invalid Item ID , please provide valid information ... !");
                }
            }
            else
            {

                return BadRequest("Invalid user Information , please provide valid information .... !");
            }
        }
        #endregion 

        #region DeleteUsers
        [HttpDelete]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _itemService.Delete(id);
                if (result == true)
                {
                    return Ok("Successfully Deleted .... !");
                }
                else
                    return BadRequest("Something Went Wrong ..... !");
            }
            else
                return BadRequest("Invalid item id, please provide valid information ...... !");
        }

        #endregion

        #endregion

        #region "Image Upload section"

        [HttpPost("FileUpload")]

        private async Task<string> UploadPhoto(IFormFile file, string Id)
        {
            string fileName;
            try
            {
                Console.WriteLine(Id);
                _logger.LogInformation("Starting uploading Item Image...!");
                //string wwwPath = this._environment.WebRootPath;
                string contentPath = this._environment.ContentRootPath;
                var extension = "." + file.FileName.Split('.')[^1];
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    fileName = Id.ToLower() + extension; //Create a new Name for the file due to security reasons.
                    string outputFileName = Regex.Replace(fileName, @"[^0-9a-zA-Z._]+", "");
                    var pathBuilt = Path.Combine(contentPath, "Images\\Item");

                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }
                    var path = Path.Combine(contentPath, "Images\\Item", outputFileName);

                    Console.WriteLine(path);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    _logger.LogInformation("Successfully uploaded Item Image with Name : " + outputFileName);
                    return outputFileName;
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Error while uploading Item Image with Name : " + ex.StackTrace);

            }

            return "";
        }
        #endregion
    }
}
