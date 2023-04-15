using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Radzen;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient{BaseAddress = new Uri(baseAddress)};
});
builder.Services.AddScoped<Alimbio.Server.alimbio_master_dbService>();
builder.Services.AddDbContext<Alimbio.Server.Data.alimbio_master_dbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("alimbio_master_dbConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("alimbio_master_dbConnection")));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderalimbio_master_db = new ODataConventionModelBuilder();
    oDataBuilderalimbio_master_db.EntitySet<Alimbio.Server.Models.alimbio_master_db.Entreprise>("Entreprises");
    oDataBuilderalimbio_master_db.EntitySet<Alimbio.Server.Models.alimbio_master_db.Salary>("Salaries");
    oDataBuilderalimbio_master_db.EntitySet<Alimbio.Server.Models.alimbio_master_db.Service>("Services");
    oDataBuilderalimbio_master_db.EntitySet<Alimbio.Server.Models.alimbio_master_db.Site>("Sites");
    oDataBuilderalimbio_master_db.EntitySet<Alimbio.Server.Models.alimbio_master_db.Ville>("Villes");
    opt.AddRouteComponents("odata/alimbio_master_db", oDataBuilderalimbio_master_db.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<Alimbio.Client.alimbio_master_dbService>();
builder.Services.AddLocalization();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRequestLocalization(options => options.AddSupportedCultures("fr").AddSupportedUICultures("fr").SetDefaultCulture("fr"));
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Run();