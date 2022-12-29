using DotNetBlog.Interfaces;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBlog.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController : Controller
{
    private readonly IPostService _postService;

    public PanelController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postService.GetAllPosts();
        return View(posts);
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
}