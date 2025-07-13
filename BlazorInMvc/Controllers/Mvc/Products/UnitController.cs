using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Interface;
using Domain.Services.Inventory;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace BlazorInMvc.Controllers.Mvc.Products
{
    public class UnitController : Controller
    {
        private readonly UnitService _unitService;
      
        public UnitController(UnitService unitService,
            IViewRenderService viewRenderService)
        {
            _unitService = unitService;
           
        }
            
        
        public async Task<IActionResult> Index()
        {
           UnitsViewModel viewModel = new UnitsViewModel();
            viewModel.UnitList =await FetchModelList();
            return PartialView("Index", viewModel);
          
        }
        public async Task<List<Unit>> FetchModelList()
        {
            var units = await _unitService.Get(
                null,                                 
                null,                               
                null,                               
                "",                                 
                GlobalPageConfig.PageNumber,        
                GlobalPageConfig.PageSize        
            );

            return units.ToList(); // Convert and return as List<Unit>
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Unit model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                // Return the AddForm partial view with validation errors
                return PartialView("_AddForm", model); // Returning partial view directly
            }

            try
            {
                if (model.UnitId > 0)
                {
                    await _unitService.Update(model);
                }
                else
                {
                    await _unitService.Save(model);
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
            if (id==0)
            {
                return NotFound();
            }
            Unit obj = (await _unitService.GetById(id));

            return PartialView("_AddForm", obj);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewForm()
        {
            Unit obj = new();
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
                if (id >0)
                {
                    var isDeleted = await _unitService.Delete(id);
                    
                }

            }
            catch (Exception)
            {
                // It's better to log the exception and provide a meaningful response to the user
                return StatusCode(500, "An error occurred while deleting the pipeline.");
            }


           
          var  list = await FetchModelList(); 

            return PartialView("_SearchResult", list);
           

        }
    }
}
