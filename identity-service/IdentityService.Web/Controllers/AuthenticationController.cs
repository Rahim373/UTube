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

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var createdUser = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = model.Email.Trim(),
                UserName = model.Email.Trim(),
                ChannelName = model.ChannelName?.Trim(),
            }, model.Password);

            if (createdUser.Succeeded)
            {
                return RedirectToAction("Login", new { model.ReturnUrl });
            }
        }

        return View(model);
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Errors"] = "Incorrect email or password";
            return View(model);
        }

        var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!signInResult.Succeeded)
        {
            ViewData["Errors"] = "Incorrect email or password";
            return View(model);
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