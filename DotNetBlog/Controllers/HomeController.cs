using System.Diagnostics;
using DotNetBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;

namespace DotNetBlog.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;
    private readonly IFileManager _fileManager;

    public HomeController(IPostService postService, IFileManager fileManager)
    {
        _postService = postService;
        _fileManager = fileManager;
    }

    public async Task<IActionResult> Index(string category)
    {
        List<Post> posts;
        if (String.IsNullOrEmpty(category))
        {
            posts = await _postService.GetAllPosts();
        }
        else
        {
            posts = await _postService.GetAllPosts(category);
        }
        return View(posts);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _postService.GetPost(id);
        return View(post);
    }

    [HttpGet("/image/{image}")]
    public IActionResult Image(string image)
    {
        var mime = image.Substring(image.LastIndexOf(".", StringComparison.Ordinal) + 1);
        return new FileStreamResult(_fileManager.ImageStream(image), $"Image/{mime}");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}