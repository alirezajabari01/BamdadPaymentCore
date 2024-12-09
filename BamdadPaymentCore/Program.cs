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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/PaymentSoapService");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.RegisterSoap();

app.MapControllers();

app.Run();
