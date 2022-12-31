using DotNetBlog.Interfaces;
using DotNetBlog.Models;
using DotNetBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBlog.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController : Controller
{
    private readonly IPostService _postService;
    private readonly IFileManager _fileManager;

    public PanelController(IPostService postService, IFileManager fileManager)
    {
        _postService = postService;
        _fileManager = fileManager;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postService.GetAllPosts();
        return View(posts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return View(new PostViewModel());
        }

        var post = await _postService.GetPost((int)id);
        if (post == null)
        {
            return View(new PostViewModel());
        }

        return View(new PostViewModel()
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            CurrentImage = post.Image
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PostViewModel vm)
    {
        try
        {
            var post = new Post()
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
            };
            if (vm.Image == null)
            {
                post.Image = vm.CurrentImage;
            }
            else
            {
                post.Image = await _fileManager.SaveImage(vm.Image);
            }

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

            return View(vm);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View(vm);
        }
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