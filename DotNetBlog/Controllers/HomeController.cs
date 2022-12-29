using System.Diagnostics;
using DotNetBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;

namespace DotNetBlog.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;

    public HomeController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postService.GetAllPosts();
        return View(posts);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _postService.GetPost(id);
        return View(post);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}