using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryController(ProductCategoryService productCategoryService
          )
        {
            _productCategoryService =productCategoryService;

        }


        public async Task<IActionResult> Index()
        {
            ProductCategoryViewModel viewModel = new ProductCategoryViewModel();
            viewModel.ProductCategories = await FetchModelList();
            return PartialView("Index", viewModel);

        }
        public async Task<List<ProductCategories>> FetchModelList()
        {
            var list = await _productCategoryService.Get(
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
        public async Task<IActionResult> SaveOrUpdate(ProductCategories model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {

                if (ImageFile != null && ImageFile.Length > 0)
                {

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(model.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/Uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    model.ImageUrl = "/assets/uploads/" + fileName;
                }


                if (model.ProdCtgId > 0)
                {
                    await _productCategoryService.Update(model);
                  

                }
                else
                {
                    long responseId = await _productCategoryService.Save(model);
                    if (responseId == -1)
                    {
                        model.ProdCtgId = 0;
                        Response.StatusCode = 409;
                        return PartialView("_AddForm", model); // Returning partial view directly

                    }
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
            ProductCategories obj = (await _productCategoryService.GetById(id));

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            ProductCategories obj = new();
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
                    var isDeleted = await _productCategoryService.Delete(id);

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
