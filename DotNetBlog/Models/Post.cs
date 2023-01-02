namespace DotNetBlog.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Tags { get; set; }

    public IEnumerable<MainComment> Comments { get; set; }

    public DateTime CreatedAt { get; set; } = new DateTime();
}