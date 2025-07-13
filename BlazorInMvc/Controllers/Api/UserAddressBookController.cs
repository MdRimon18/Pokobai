using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorInMvc.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAddressBookController : ControllerBase
    {
        private readonly UserAddressBookService _userAddressBookService;

        public UserAddressBookController(UserAddressBookService userAddressBookService)
        {
            _userAddressBookService = userAddressBookService;
        }

        // GET: api/UserAddressBook/GetAllAddresses
        [HttpGet("GetAllAddresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _userAddressBookService.GetAllAddressesAsync();
            return Ok(new
            {
                response = addresses.Select(address => new
                {
                    addressID = address.AddressID,
                    userID = address.UserID,
                    addressType = address.AddressType,
                    address = address.Address,
                    city = address.City,
                    state = address.State,
                    postalCode = address.PostalCode,
                    phoneNumber = address.PhoneNumber,
                    country = address.Country,
                    isDefault = address.IsDefault
                }).ToList(),
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }
        [HttpGet("GetAddressByUserId")]
        public async Task<IActionResult> GetAddressByUserId(long userId)
        {
            var addresses = await _userAddressBookService.GetAllAddressesByUserIdAsync(userId);
            return Ok(new
            {
                response = addresses.Select(address => new
                {
                    addressID = address.AddressID,
                    userID = address.UserID,
                    addressType = address.AddressType,
                    address = address.Address,
                    city = address.City,
                    state = address.State,
                    postalCode = address.PostalCode,
                    phoneNumber = address.PhoneNumber,
                    country = address.Country,
                    isDefault = address.IsDefault
                }).ToList(),
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }

        // GET: api/UserAddressBook/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _userAddressBookService.GetAddressByIdAsync(id);
            if (address == null)
            {
                return NotFound(new
                {
                    response = (object)null,
                    code = HttpStatusCode.NotFound,
                    message = "Address not found",
                    isSuccess = false
                });
            }

            return Ok(new
            {
                response = new
                {
                    addressID = address.AddressID,
                    userID = address.UserID,
                    addressType = address.AddressType,
                    address = address.Address,
                    city = address.City,
                    state = address.State,
                    postalCode = address.PostalCode,
                    phoneNumber = address.PhoneNumber,
                    country = address.Country,
                    isDefault = address.IsDefault
                },
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }

        // POST: api/UserAddressBook
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateAddress([FromBody] UserAddressBook address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    response = ModelState,
                    code = HttpStatusCode.BadRequest,
                    message = "Invalid data",
                    isSuccess = false
                });
            }

         //   address.UserID = UserInfo.UserId;
            var createdAddress = await _userAddressBookService.CreateOrUpdateAddressAsync(address);
            return Ok(new
            {
                response = new
                {
                    addressID = createdAddress.AddressID,
                    userID = createdAddress.UserID,
                    addressType = createdAddress.AddressType,
                    address = createdAddress.Address,
                    city = createdAddress.City,
                    state = createdAddress.State,
                    postalCode = createdAddress.PostalCode,
                    phoneNumber = createdAddress.PhoneNumber,
                    country = createdAddress.Country,
                    isDefault = createdAddress.IsDefault
                },
                code = HttpStatusCode.OK,
                message = "Success",
                isSuccess = true
            });
        }
    }
}
