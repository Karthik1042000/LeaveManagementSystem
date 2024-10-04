using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeaveManagementSystem.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if(userRole == "Admin")
            {
                ViewBag.Data = true;
            }
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignIn signIn)
        {
            var user = await employeeRepository.SignInAsync(signIn.Id, signIn.Password);

            if (user == null)
            {
                return BadRequest("Invalid credentials.");
            }
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Json(user);
        }

        [HttpGet]
        public IActionResult CachedDetails()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return Json(new { userId , userRole });
        }

        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
