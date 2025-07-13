using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class ProductColorController : Controller
    {
        private readonly ColorService _colorService;

        public ProductColorController(ColorService colorService
          )
        {
            _colorService = colorService;

        }


        public async Task<IActionResult> Index()
        {
            ProductColorViewModel viewModel = new ProductColorViewModel();
            viewModel.Color_list = await FetchModelList();
            return PartialView("Index", viewModel);

        }
        public async Task<List<Colors>> FetchModelList()
        {
            var list = await _colorService.Get(
                null,
                null,
                null,
                null,
                GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize
            );

            return list.ToList(); // Convert and return as List<Unit>
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Colors model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {

                long responseId = await _colorService.SaveOrUpdate(model);
                if (responseId == -1)
                { 
                    model.ColorId = 0;
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
            Colors obj = (await _colorService.GetById(id));

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            Colors obj = new();
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
                    var isDeleted = await _colorService.Delete(id);

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
