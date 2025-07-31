using Domain.CommonServices;
using Domain.Entity;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorInMvc.Controllers.Api
{
    [Route("api/[controller]")]
    public class ItemCardController : BaseController
    {
        private readonly ItemCardService _itemCardService;

        public ItemCardController(ItemCardService itemCardService)
        {
            _itemCardService = itemCardService;
        }

        [HttpGet("GetItemCart")]
        public async Task<IActionResult> GetItemCart(long companyId,
            int? cartId = null,
            string?browserId=null,
            long? customerId = null,
            long? productId = null,
            string? sku = null)
        {
            try
            {
               // CompanyInfo.CompanyId = companyId;
                var result = (await _itemCardService.GetItemCartAsync(cartId, browserId, customerId, productId, sku)).ToList();
                if (result == null || !result.Any())
                {
                    return ErrorMessage("No items found in the cart.");
                }
                return Ok(new
                {
                    result =result,
                    code = (int)HttpStatusCode.OK,
                    message = "success",
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError(ex);
            }
        }

        [HttpPost("SaveOrUpdateItemCart")]
        public async Task<IActionResult> SaveOrUpdateItemCart([FromBody] ItemCart itemCart)
        {
            if (itemCart == null)
            {
                return ErrorMessage("Invalid item cart data.");
            }

            try
            {
                itemCart.CompanyId = itemCart.CompanyId;
                var successId = await _itemCardService.SaveOrUpdateItemCart(itemCart);

                if (successId > 0)
                {
                    return SuccessMessage(new { CartId = successId });
                }
                else
                {
                    return ErrorMessage("Failed to save or update the item cart.");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError(ex);
            }
        }
        [HttpGet("UpdateItemQuantity")]
        public async Task<IActionResult> UpdateItemQuantity(int id, int quantity)
        {
            if (id<=0 && quantity<=0)
            {
                return ErrorMessage("Quantity Or Id Is Not Valid");
            }

            try
            {
                var itemCart = (await _itemCardService.GetItemCartAsync(id, null, null, null))?.FirstOrDefault();
                
                if (itemCart != null)
                {
                    itemCart.Quantity = quantity;
                    var successId = await _itemCardService.SaveOrUpdateItemCart(itemCart);

                    if (successId > 0)
                    {
                        return SuccessMessage(new { CartId = successId });
                    }
                    else
                    {
                        return ErrorMessage("Failed to save or update the item cart.");
                    }
                }
                return ErrorMessage("Failed to save or update the item cart.");

            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError(ex);
            }
        }
        [HttpGet("DeleteItemCart")]
        public async Task<IActionResult> DeleteItemCart(
            int? cartId = null,
            string? browserId=null,
            int? customerId = null,
            int? productId = null,
            string sku = null)
        {
            if (cartId == null && customerId == null && productId == null && string.IsNullOrEmpty(sku))
            {
                return ErrorMessage("At least one parameter must be provided for deletion.");
            }

            try
            {
                var isDeleted = await _itemCardService.DeleteItemCart(cartId, browserId, customerId, productId, sku);

                if (isDeleted)
                {
                    return SuccessMessage("Item cart deleted successfully.");
                }
                else
                {
                    return ErrorMessage("Failed to delete item cart.");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError(ex);
            }
        }
    }
}
