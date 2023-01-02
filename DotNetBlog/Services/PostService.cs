using DotNetBlog.Data;
using DotNetBlog.Interfaces;
using DotNetBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetBlog.Services;

public class PostService : IPostService
{
    private readonly AppDbContext _context;

    public PostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetPost(int id)
    {
        return await _context.Posts.Where(x => x.Id == id)
            .Include(x => x.Comments)
            .ThenInclude(mc => mc.SubComments)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Post>> GetAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<List<Post>> GetAllPosts(string category)
    {
        return await _context.Posts.Where(x => x.Category.ToLower().Equals(category.ToLower())).ToListAsync();
    }

    public void AddPost(Post post)
    {
        _context.Posts.Add(post);
    }

    public void UpdatePost(Post post)
    {
        _context.Posts.Update(post);
    }

    public async Task RemovePost(int id)
    {
        var post = await GetPost(id);
        if (post == null)
        {
            return;
        }

        _context.Posts.Remove(post);
    }

    public void CreateComment(MainComment comment)
    {
        _context.MainComments.Add(comment);
    }

    public async Task UpdateComment(int id, string message)
    {
        var comment = await _context.MainComments.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (comment == null)
        {
            return;
        }

        _context.SubComments.Add(new SubComment()
        {
            MainCommentId = id,
            Message = message
        });
    }

    public async Task<bool> SaveChangesAsync()
    {
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }

        return false;
    }
}