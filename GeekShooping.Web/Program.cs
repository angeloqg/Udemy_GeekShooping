using GeekShooping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HttpClient configuration
var configuration = builder.Configuration["ServiceUrls:ProductApi"];

builder.Services.AddHttpClient<IProductService, ProductService>(
    c => c.BaseAddress = new Uri(configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
