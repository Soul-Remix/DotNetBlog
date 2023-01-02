namespace DotNetBlog.Models;

public class MainComment : Comment
{
    public List<SubComment> SubComments { get; set; } = new();
    public int PostId { get; set; }
}