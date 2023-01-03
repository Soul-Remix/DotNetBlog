using DotNetBlog.Models;

namespace DotNetBlog.ViewModels;

public class PaginatedPostViewModel
{
    public IEnumerable<Post> Posts { get; set; }
    public int PageNumber { get; set; }
    public bool HasNextPage { get; set; }
}