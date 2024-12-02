using BamdadPaymentCore;
using BamdadPaymentCore.Domain;
using BamdadPaymentCore.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptionPattern();
builder.Services.AddControllersWithViews();
builder.Services.RegisterSoapServices();
builder.Services.RegisterDomainDependencies();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
   
}

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.RegisterSoap();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
