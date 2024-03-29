﻿using DotNetBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DotNetBlog.Models;
using DotNetBlog.ViewModels;

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

    public async Task<IActionResult> Index(string category, int pageNum, string search)
    {
        var result = await _postService.GetAllPosts(pageNum, category, search);

        return View(result);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _postService.GetPost(id);
        return View(post);
    }

    [HttpGet("/image/{image}")]
    [ResponseCache(CacheProfileName = "Monthly")]
    public IActionResult Image(string image)
    {
        var mime = image.Substring(image.LastIndexOf(".", StringComparison.Ordinal) + 1);
        return new FileStreamResult(_fileManager.ImageStream(image), $"Image/{mime}");
    }

    [HttpPost]
    public async Task<IActionResult> Comment(CommentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return await Post(vm.PostId);
        }

        if (vm.MainCommentId == 0)
        {
            var post = await _postService.GetPost(vm.PostId);
            if (post == null)
            {
                return await Post(vm.PostId);
            }

            var mainComment = new MainComment()
            {
                Message = vm.Message,
                PostId = vm.PostId
            };

            _postService.CreateComment(mainComment);
        }
        else
        {
            await _postService.UpdateComment(vm.MainCommentId, vm.Message);
        }

        await _postService.SaveChangesAsync();

        return RedirectToAction(nameof(Post), new { Id = vm.PostId });
    }
}