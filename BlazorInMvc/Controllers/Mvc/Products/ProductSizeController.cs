using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Interface;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class ProductSizeController : Controller
    {
        private readonly ProductSizeService _productSizeService;
        
        public ProductSizeController(ProductSizeService unitService
          )
        {
            _productSizeService = unitService;
           
        }


        public async Task<IActionResult> Index()
        {
            ProductSizeViewModel viewModel = new ProductSizeViewModel();
            viewModel.ProductSizeList = await FetchModelList();
            return PartialView("Index", viewModel);

        }
        public async Task<List<ProductSze>> FetchModelList()
        {
            var list = await _productSizeService.Get(
                null,
                null,
                null,
                "",
                GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize
            );

            return list.ToList(); // Convert and return as List<Unit>
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(ProductSze model)
            {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {

                long responseId= await _productSizeService.SaveOrUpdate(model);
                if (responseId ==-1) {
                    model.rowsAffected = -1;
                    model.ProductSizeId =0;
                    Response.StatusCode = 409;
                    return PartialView("_AddForm", model); // Returning partial view directly
                 
                }

                    
                var list = await FetchModelList();
                // Return the _SearchResult partial view with the updated list
                return PartialView("_SearchResult", list); // Returning partial view directly
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                // In case of an error, render the AddForm partial view again
                return PartialView("_AddForm", model); // Returning partial view directly
            }
        }


        [HttpGet]
        public async Task<IActionResult> LoadEditModeData(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ProductSze obj = (await _productSizeService.GetById(id));

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            ProductSze obj = new();
            if (obj == null)
            {
                return NotFound();
            }

            return PartialView("_AddForm", obj);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid ID provided.");
            }

            try
            {
                if (id > 0)
                {
                    var isDeleted = await _productSizeService.Delete(id);

                }

            }
            catch (Exception)
            {
                // It's better to log the exception and provide a meaningful response to the user
                return StatusCode(500, "An error occurred while deleting the pipeline.");
            }



            var list = await FetchModelList();

            return PartialView("_SearchResult", list);


        }
    }
}
