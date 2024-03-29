using System.ComponentModel.DataAnnotations;

namespace DotNetBlog.ViewModels;

public class LogInViewModel
{
    [Required] public string UserName { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
}