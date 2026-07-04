using ERP.Data;
using ERP.Models;
using Microsoft.EntityFrameworkCore; // Enables SqlServer + EF configuration

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//ApplicationDbContext registers Db service in the app
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => sql.EnableRetryOnFailure()));
//tells EF to use SQL server

//Authentication - custom session - based authentication
//registers session service - allows app to store temporary user data between requests 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
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

//Authentication - enables session middleware in the request pipeline
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

//enables api routes /api/staff
app.MapControllers();

//HomeCOntroller becomes - /Home/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
