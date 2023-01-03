using DotNetBlog.Models;
using DotNetBlog.ViewModels;

namespace DotNetBlog.Interfaces;

public interface IPostService
{
    Task<Post?> GetPost(int id);
    Task<List<Post>> GetAllPosts();
    Task<PaginatedPostViewModel> GetAllPosts(int pageNum,string category);
    void AddPost(Post post);
    void UpdatePost(Post post);
    Task RemovePost(int id);
    public void CreateComment(MainComment comment);
    public Task UpdateComment(int id, string message);
    Task<bool> SaveChangesAsync();
}