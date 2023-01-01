using DotNetBlog.Models;

namespace DotNetBlog.Interfaces;

public interface IPostService
{
    Task<Post?> GetPost(int id);
    Task<List<Post>> GetAllPosts();
    Task<List<Post>> GetAllPosts(string category);
    void AddPost(Post post);
    void UpdatePost(Post post);
    Task RemovePost(int id);
    Task<bool> SaveChangesAsync();
}