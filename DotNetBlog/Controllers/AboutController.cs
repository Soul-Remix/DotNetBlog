using Microsoft.AspNetCore.Mvc;

namespace DotNetBlog.Controllers;

public class AboutController :Controller
{
    public IActionResult Privacy()
    {
        return View();
    }
}