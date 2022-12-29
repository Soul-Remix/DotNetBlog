using DotNetBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBlog.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        return View(new LogInViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel vm)
    {
        var result = await _signInManager
            .PasswordSignInAsync(vm.UserName, vm.Password, false, false);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}