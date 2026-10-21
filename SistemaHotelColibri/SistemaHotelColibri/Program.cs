using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.BusinessLogic;
using SistemaHotelColibri.DataAccess;
using SistemaHotelColibri.DataAccess.Identidad;
using SistemaHotelColibri.Filtros;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddBusinessLogic();

builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.Cookie.Name = "Colibri.Sesion";
    opciones.Cookie.HttpOnly = true;
    opciones.LoginPath = "/Cuenta/IniciarSesion";
    opciones.LogoutPath = "/Cuenta/CerrarSesion";
    opciones.AccessDeniedPath = "/Cuenta/AccesoDenegado";
    opciones.ExpireTimeSpan = TimeSpan.FromHours(8);
    opciones.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    opciones.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
    opciones.Filters.Add<RequiereCambioContrasenaFiltro>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var contrasenaPruebas = app.Configuration["Semilla:ContrasenaUsuariosPrueba"];
    if (!string.IsNullOrWhiteSpace(contrasenaPruebas))
    {
        await app.Services.SembrarContrasenasAsync(contrasenaPruebas);
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var cultura = CulturaSistema.Crear();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = [cultura],
    SupportedUICultures = [cultura],
    RequestCultureProviders = []
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
