using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityService.Data.Models;
using IdentityService.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Web.Controllers;

public class AuthenticationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login(string returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!signInResult.Succeeded)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        var isuser = new IdentityServerUser(user.Id.ToString())
        {
            DisplayName = user.UserName
        };

        var props = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(300))
        };

        await HttpContext.SignInAsync(isuser, props);

        if (!string.IsNullOrEmpty(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (User.IsAuthenticated())
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
        }

        return Ok();
    }
}