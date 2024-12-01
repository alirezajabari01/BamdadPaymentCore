using BamdadPaymentCore;
using BamdadPaymentCore.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptionPattern();
builder.Services.AddControllersWithViews();
builder.Services.RegisterSoap();
builder.Services.RegisterDomainDependencies();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
   
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.RegisterSoap();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
