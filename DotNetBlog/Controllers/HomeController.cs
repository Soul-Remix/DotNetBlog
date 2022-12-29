using System.Diagnostics;
using DotNetBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;

namespace DotNetBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;

    public HomeController(ILogger<HomeController> logger, IPostService postService)
    {
        _logger = logger;
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


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (id > 0)
        {
            var post = await _postService.GetPost(id);
            return View(post);
        }

        return View(new Post());
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Post post)
    {
        if (post.Id > 0)
        {
            _postService.UpdatePost(post);
        }
        else
        {
            _postService.AddPost(post);
        }

        var success = await _postService.SaveChangesAsync();
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var post = await _postService.GetPost(id);
        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        await _postService.RemovePost(id);
        await _postService.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}