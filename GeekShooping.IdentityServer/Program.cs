using Duende.IdentityServer.Services;
using GeekShooping.IdentityServer.Configuration;
using GeekShooping.IdentityServer.Initializer;
using GeekShooping.IdentityServer.Model;
using GeekShooping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Connection Database
var connection = builder.Configuration["MySqlConnection:MySqlConnetionString"];

builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(connection,
                                                       ServerVersion.AutoDetect(connection)));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MySqlContext>()
                .AddDefaultTokenProviders();

 var builderIdentity = builder.Services.AddIdentityServer(options =>
     {
         options.Events.RaiseErrorEvents = true;
         options.Events.RaiseInformationEvents = true;
         options.Events.RaiseFailureEvents = true;
         options.Events.RaiseSuccessEvents = true;
         options.EmitStaticAudienceClaim = true;
     }).AddInMemoryIdentityResources(
                             IdentityConfiguration.IdentityResources)
                         .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                         .AddInMemoryClients(IdentityConfiguration.Clients)
                         .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builderIdentity.AddDeveloperSigningCredential();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetService<IDbInitializer>();

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

app.UseIdentityServer();

app.UseAuthorization();

service?.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
