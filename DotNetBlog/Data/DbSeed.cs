using Microsoft.AspNetCore.Identity;

namespace DotNetBlog.Data;

public static class DbSeed
{
    public static void Initialize(IServiceProvider provider)
    {
        var ctx = provider.GetRequiredService<AppDbContext>();
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        ctx.Database.EnsureCreated();

        if (!roleManager.Roles.Any())
        {
            var role = new IdentityRole("Admin");
            roleManager.CreateAsync(role).GetAwaiter().GetResult();
        }

        if (!userManager.Users.Any())
        {
            var user = new IdentityUser()
            {
                UserName = "AdminUser",
                Email = "admin@test.com"
            };

            userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
        }

        if (ctx.Posts.Any())
        {
            return;
        }
    }
}