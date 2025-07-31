using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.RequestModel;
using Domain.Services;
using Domain.Services.Inventory;
using Domain.Services.Settings;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Net;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserService _userService;
        private readonly UserAddressBookService _userAddressBookService;
        private readonly ItemCardService _itemCardService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly InvoiceService _invoiceService;
        public UserController(UserService userService, UserAddressBookService userAddressBookService, 
            ItemCardService itemCardService, InvoiceItemService invoiceItemService, InvoiceService invoiceService)
        {
            _userService = userService;
            _userAddressBookService = userAddressBookService;
            _itemCardService = itemCardService;
            _invoiceItemService = invoiceItemService;
            _invoiceService = invoiceService;
        }
        [HttpGet("GetCurrentUserId")]
        public IActionResult GetCurrentUserId()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
 
            return Ok(new { CurrentUserId = userId });
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUsers(string? search,long roleId, int page, int pageSize)
        {
            // Sending null for all filters
            var users = (await _userService.Get(
                userId: null,
                email: null,
                name: search,
                phoneNo: null,
                password: null,
                roleId: roleId,
                pageNumber: page,
                pageSize: pageSize
            )).ToList();
          //   var test =users.Where(w=>w.Address is not null).ToList();
            if (users.Count == 0)
            {
                return Ok(new
                {
                    items = users,
                    currentPage = page,
                    totalPages = 0,
                    totalRecord = 0
                });
            }

            var totalRecord = users[0].total_row;
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                items = users,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(long id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Route("SaveUser")]
        public async Task<ActionResult<User>> SaveUser(User user,IFormFile? imageFile)
        {
            try
            {
                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/Users/Images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    user.ImgLink = "/Users/images/" + fileName;
                }
                
                if (user.UserId > 0)
                {
                    user.LastModifyDate = DateTime.UtcNow;
                    user.LastModifyBy =User.GetUserId(); // Replace this with your logic
                    user.CompanyId = User.GetCompanyId();
                }
                else
                {
                    user.CompanyId = User.GetCompanyId();
                    user.EntryDateTime = DateTime.UtcNow;
                    user.EntryBy = User.GetUserId(); // Replace this with your logic

                   
                }
              long successId = await _userService.SaveOrUpdate(user);
                if (successId == -1) { return BadRequest("Phone No Exist."); }
                if (successId == -2) { return BadRequest("Email Already Exist."); }
                if (successId == -3) { return BadRequest("Phone or Email is Required."); }
                //successId -1 phone exist
                //successId -2 email exist,
                //successId -3 Pone or Email Required
                if (successId > 0)
                {
                  user.UserId = successId;
                  return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
                     
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save user");
            }
            return BadRequest("Failed to save user");


        }


        [HttpPost]
        [Route("SaveUserForEcommerce")]
        public async Task<ActionResult<User>> SaveUserForEcommerce(UserRequestModel userRequest)
        {
            if (userRequest.CompanyId <= 0)
            {
                return BadRequest("Company Id is Required.");
            }
            if (string.IsNullOrEmpty(userRequest.Name))
            {
                return BadRequest("Name is Required.");
            }
            try

            {
                
                //check existing user by email or phone


                // Handle image upload
                //if (imageFile != null)
                //{
                //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                //    var filePath = Path.Combine("wwwroot/Users/Images", fileName);
                //    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                //    using var stream = new FileStream(filePath, FileMode.Create);
                //    await imageFile.CopyToAsync(stream);

                //    userRequest.ImgLink = "/Users/images/" + fileName;
                //}

                // Map to User entity

                var user = new User
                {
                    UserId = userRequest.UserId, 
                    Name = userRequest.Name,
                    PhoneNo = userRequest.PhoneNo,
                    Email = userRequest.Email,
                    RoleId = SelectedUserRole.CustomerRoleId,
                    CompanyId = userRequest.CompanyId, // Override from context
                    BranchId = userRequest.BranchId,
                    CountryId = CompanyInfo.CountryId,
                    MembershipId = userRequest.MembershipId,
                    IsAbleToLogin = userRequest.IsAbleToLogin,
                    ImgLink = userRequest.ImgLink,
                };

                if (user.UserId > 0)
                {
                    user.LastModifyDate = DateTime.UtcNow;
                    user.LastModifyBy = UserInfo.UserId;
                }
                else
                {
                    user.EntryDateTime = DateTime.UtcNow;
                    user.EntryBy = UserInfo.UserId;
                }

                long successId = await _userService.SaveOrUpdate(user);
                if (successId == -1) {
                  var existUser=await _userService.GetByPhone(user.PhoneNo);
                    return existUser==null?BadRequest("User Not Found."): existUser;
                    //return BadRequest("Phone No Exist.")
                }
                ;
                if (successId == -2)
                {
                    var existUser = await _userService.GetByEmail(user.Email);
                    return existUser == null ? BadRequest("User Not Found.") : existUser;
                    //return BadRequest("Phone No Exist.")
                }
                if (successId == -3) return BadRequest("Phone or Email is Required.");

                if (successId > 0)
                {
                     user.UserId = successId;
                    return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Failed to save user");
            }
        }


        [HttpPost]
        [Route("SaveUserForEcommercewithAddress")]
        public async Task<ActionResult<UserRequestModelWithAddress>> SaveUserForEcommercewithAddress(UserRequestModelWithAddress userRequest)
        {
            if (userRequest.CompanyId <= 0)
                return BadRequest("Company Id is required.");

            if (string.IsNullOrWhiteSpace(userRequest.Name))
                return BadRequest("Name is required.");

            try
            {
                var user = new User
                {
                    Name = userRequest.Name,
                    PhoneNo = userRequest.PhoneNo,
                    Email = null,
                    RoleId = SelectedUserRole.CustomerRoleId,
                    CompanyId = userRequest.CompanyId,
                    BranchId = 0,
                    CountryId = CompanyInfo.CountryId,
                    IsAbleToLogin = true,
                    EntryDateTime = DateTime.UtcNow,
                    EntryBy = UserInfo.UserId,
                    Status = "Active"
                };

                long resultUserId = await _userService.SaveOrUpdate(user);

                if (resultUserId == -3)
                    return BadRequest("Phone or Email is required.");

                // Get existing user (either just saved or previously existed)
                var existingUser = await _userService.GetByPhone(user.PhoneNo);
                if (existingUser == null)
                    return BadRequest("Failed to retrieve user after save.");

                userRequest.UserId = existingUser.UserId;

                // Handle Address
                var address = await _userAddressBookService.GetAddressByPhoneAsync(existingUser.PhoneNo);

                if (address != null)
                {
                    address.Address = userRequest.Address;
                    address.City = userRequest.City ?? address.City;
                    //address.Country = userRequest.C ?? "Bangladesh";
                    address.LastModifyDate = DateTime.UtcNow;
                    address.LastModifyBy = existingUser.UserId;
                }
                else
                {
                    address = new UserAddressBook
                    {
                        UserID = existingUser.UserId,
                        AddressType = "Home",
                        Address = userRequest.Address,
                        City = userRequest.City,
                        Country ="Bangladesh",
                        PhoneNumber = existingUser.PhoneNo,
                        IsDefault = true,
                        EntryDateTime = DateTime.UtcNow,
                        EntryBy = existingUser.UserId,
                        Status = "Active"
                    };
                }

                var savedAddress = await _userAddressBookService.CreateOrUpdateAddressAsync(address);
                userRequest.AddressID = savedAddress.AddressID;

                if (userRequest==null)
                    return BadRequest("User Not Found.");

                if (string.IsNullOrEmpty(userRequest.BrowserId))
                    return BadRequest("Browser Id Not Found.");
                //place Order 

                var itemCartList = (await _itemCardService.GetItemCartAsync(null,userRequest.BrowserId, null, null, null)).ToList();

                if (!itemCartList.Any())
                    return BadRequest("No items in cart to place an order.");

                var invoiceItems = itemCartList.Select(item => new InvoiceItems
                {
                    ProductImage = item.ImageUrl,
                    ProductName = item.ProductName,
                    CategoryName = item.ProdCtgName,
                    SubCtgName = item.ProdSubCtgName,
                    Unit = item.UnitName,
                    InvoiceId = 0,
                    ProductId = item.ProductId,
                    Quantity = (int)item.Quantity,
                    SellingPrice = item.Price,
                    BuyingPrice = item.BuyingPrice ?? 0,
                    DiscountPercentg = 0,
                    RowIndex = item.CartId,
                    Status = "Active",
                    ProductVariantId = item.ProductVariantId ?? 0
                }).ToList();

                var totalQuantity = invoiceItems.Sum(x => x.Quantity);
                var totalAmount = invoiceItems.Sum(x => x.SellingPrice * x.Quantity);
                var totalVat = 0M;
                var totalDiscount = invoiceItems.Sum(x => ((x.SellingPrice * x.DiscountPercentg) / 100M) * x.Quantity);
                var totalAddiDiscount = 0M;
                var totalPayable = totalAmount - totalDiscount - totalAddiDiscount;
                var receiveAmount = totalPayable;
                var dueAmount = totalPayable - receiveAmount;

                var invoiceSummary = new
                {
                    TotalQuantity = totalQuantity,
                    TotalAmount = totalAmount,
                    TotalVat = totalVat,
                    TotalDiscount = totalDiscount,
                    TotalAddiDiscount = totalAddiDiscount,
                    TotalPayable = totalPayable,
                    RecieveAmount = receiveAmount,
                    DueAmount = dueAmount
                };

                var invoice = new Invoice
                {
                    InvoiceId = 0,
                    CompanyId = userRequest.CompanyId,
                    BranchId = 0,
                    InvoiceNumber = "E-1",
                    CustomerID = userRequest.UserId,
                    InvoiceDateTime = DateTime.UtcNow,
                    InvoiceTypeId = 10002,
                    NotificationById = 1,
                    SalesByName = "",
                    Notes = "",
                    PaymentTypeId = 10002,
                    TotalQnty = (int)invoiceSummary.TotalQuantity,
                    TotalAmount = (decimal)invoiceSummary.TotalAmount,
                    TotalVat = (decimal)invoiceSummary.TotalVat,
                    TotalDiscount = (decimal)invoiceSummary.TotalDiscount,
                    TotalAddiDiscount = (decimal)invoiceSummary.TotalAddiDiscount,
                    TotalPayable = (decimal)invoiceSummary.TotalPayable,
                    RecieveAmount = (decimal)invoiceSummary.RecieveAmount,
                    DueAmount = (decimal)invoiceSummary.DueAmount,
                    DeliveryAddressId =userRequest.AddressID,
                    Status = "Active",
                    EntryDateTime = DateTime.UtcNow,
                    EntryBy = userRequest.UserId,
                    total_row = invoiceItems.Count
                };
                long newInsertedInvoiceId = await _invoiceService.SaveOrUpdate(invoice);
                if (newInsertedInvoiceId == 0)
                {
                    return BadRequest("Failed to save invoice.");
                }
                foreach (var invoiceItem in invoiceItems)
                {
                    invoiceItem.InvoiceId = newInsertedInvoiceId;
                    var itemId = await _invoiceItemService.SaveOrUpdate(invoiceItem);
                    if (itemId == 0)
                    {
                        return BadRequest($"Failed to save invoice item for ProductId {invoiceItem.ProductId}.");
                    }
                    invoiceItem.InvoiceItemId = itemId;
                    userRequest.OrderId= newInsertedInvoiceId;
                }

                if (newInsertedInvoiceId > 0&&!string.IsNullOrEmpty(userRequest.BrowserId))
                {
                   await _itemCardService.DeleteItemCart(browserId: userRequest.BrowserId);
                }

                return Ok(userRequest);
            }
            catch (Exception ex)
            {
                // Log the error for debugging (if logging is available)
                return BadRequest("Failed to save user and address. Error: " + ex.Message);
            }
        }

    }



}
