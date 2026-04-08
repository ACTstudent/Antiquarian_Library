using Antiquarian_Library.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<Antiquarian_Library.Services.LocalDatabaseService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/";
        options.LogoutPath = "/User/Logout";
        options.Cookie.Name = "AntiquarianAuth";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    // Seed the admin user if it doesn't exist
    if (!context.Users.Any(u => u.Username == "admin1"))
    {
        var adminUser = new Antiquarian_Library.Models.User
        {
            Id = Guid.NewGuid().ToString(),
            Username = "admin1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = "Administrator"
        };
        context.Users.Add(adminUser);
        context.SaveChanges();
    }
}

app.Run();
