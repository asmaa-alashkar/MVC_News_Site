using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_News.Models;
using System.Security.Claims;

namespace MVC_News.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
                if (model.Name=="Asmaa")
                {
                    Claim claim = new Claim(ClaimTypes.Role, "admin");
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(claim);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    //if (returnURL != null)
                    //{
                    //    return LocalRedirect(returnURL);
                    //}

                    return RedirectToAction("Index", "News");
                }              
            
            ModelState.AddModelError("", "invalid user name or password");
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();
            
            return RedirectToAction("Index", "News");
        }
    }
}
