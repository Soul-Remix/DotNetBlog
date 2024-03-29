using DotNetBlog.Data;
using DotNetBlog.Interfaces;
using DotNetBlog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSauce.MagicScaler;
using PhotoSauce.NativeCodecs.Libjpeg;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Data")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 5;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Auth/Login"; });

builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IFileManager, FileManager>();

builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("Monthly", new CacheProfile() { Duration = 60 * 60 * 24 * 7 * 4 });
});

CodecManager.Configure(codecs => { codecs.UseLibjpeg(); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    DbSeed.Initialize(provider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();