namespace DotNetBlog.Models;

public class SubComment : Comment
{
    public int MainCommentId { get; set; }
}