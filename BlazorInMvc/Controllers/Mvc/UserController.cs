using Domain.CommonServices;
using Domain.Entity.Settings;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Mvc
{
    public class UserController : Controller
    {
        public IActionResult Index(long roleId)
        {
            var model = new User();
            model.RoleId = roleId;
            model.PageName = RoleHelper.GetRoleWiseListPageName(roleId);
         
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Index",model); // Return partial view for AJAX requests
            }

            return View("Index", model);
        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid? key,long roleId)
        {
            var model = new User();
            model.RoleId = roleId;
            model.RoleName=RoleHelper.GetRoleName(roleId);
            model.CountryId = CompanyInfo.CountryId;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Create", model); // Return partial view for AJAX requests
            }

            return View("Create", model);
        }
    }
}
