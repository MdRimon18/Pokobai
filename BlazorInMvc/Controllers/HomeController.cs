using BlazorInMvc.Models;
using Domain.CommonServices;
using Domain.Entity.Settings;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BlazorInMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly CompanyService _companyService;

        public HomeController(ILogger<HomeController> logger,
            UserService userService,
            CompanyService companyService)
        {
            _logger = logger;
            _userService = userService;
            _companyService = companyService;
        }

        public IActionResult Index(string component = null, bool isPartial = false)
        {
            ViewData["Component"] = component;

            if (isPartial)
            {
                return PartialView("Index");
            }

            return View();
        }

        public IActionResult SignUp(bool isPartial = false)
        {
           

            if (isPartial)
            {
                return PartialView("SignUp");
            }
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(User model)
        {   
            long userId=  await  _userService.SaveOrUpdate(model);
            return View(model);
        }
        public IActionResult Login(bool isPartial = false)
        {


            if (isPartial)
            {
                return PartialView("Login");
            }

            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> Login(User user, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(user.PhoneNo)|| string.IsNullOrEmpty(user.Password))
            {
                return View(user);
            }

            var existingUser = await _userService.GetByPhone(user.PhoneNo);

            if (existingUser == null)
            {
                ViewData["ErrorMessage"] = "Invalid phone number or password, or account is disabled.";
                return View(user);
            }
            if(existingUser.Password != user.Password)
            {
                ViewData["ErrorMessage"] = "Invalid phone number or password.";
                return View(user);
            }
            Company company = new Company();
            if (existingUser.CompanyId>0)
            {
                company = await _companyService.GetById((long)existingUser.CompanyId);
            }
            var request = HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";

            // Create claims for the authenticated user
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, existingUser.UserId.ToString()),
            new Claim(ClaimTypes.Name, existingUser.Name),
            new Claim(ClaimTypes.Role, existingUser.RoleName ?? "User"),
            new Claim("PhoneNo", existingUser.PhoneNo),
            new Claim("CompanyId", existingUser.CompanyId==null?"0":existingUser.CompanyId.ToString()),
            new Claim("CompanyLogoImgLink", baseUrl + (company?.CompanyLogoImgLink ?? "")),
            new Claim("UserImage", baseUrl + (existingUser?.ImgLink ?? "")),
            new Claim("CompanyName", company?.CompanyName),

        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = user.RememberMe,
                ExpiresUtc = user.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
            };

            // Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect to returnUrl or home page
            returnUrl = returnUrl ?? Url.Content("/Dashboard/Index");
            return LocalRedirect(returnUrl);

            // to get access the claim data 

           // var companyId = User.GetCompanyId();
            //var userId = User.GetUserId();
            //var userName = User.GetName();
            //var roleName = User.GetRoleName();
            //var phoneNo = User.GetPhoneNo();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ThankYou(long? orderId)
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOrderDetails()
        {
            var orderDetails = new
            {
                OrderId = "ORD123456",
                EstimatedDelivery = "3-5 Business Days",
                PaymentStatus = "Paid"
            };
            return Json(orderDetails);
        }
        public IActionResult Privacy()
        {
            return PartialView("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
