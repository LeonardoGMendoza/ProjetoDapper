using CrudDapperMvc.Business;
using CrudDapperMvc.Model.Interfaces;
using CrudDapperMvc.Repository.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Obter a configuração
var configuration = builder.Configuration;

// Obter a string de conexão do arquivo appsettings.json
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Register services
builder.Services.AddTransient<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddTransient<UserBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllers(routes =>
//{
//    routes.MapControllerRoute(
//        name: "user",
//        pattern: "User/{action}/{id?}",
//        defaults: new { controller = "User", action = "Index" });
//});

app.MapControllerRoute(
    name: "user",
    pattern: "User/{action}/{id?}",
    defaults: new { controller = "User", action = "Index" });



app.Run();
