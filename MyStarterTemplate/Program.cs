using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MyStarterTemplate.Data;
using MyStarterTemplate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Identity", "/Admin", "adminsOnly");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login");
});

builder.Services.AddServerSideBlazor();

// Add Identity services, order is important, factory first!
var cs = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContextFactory<DataContext>(options => options.UseSqlServer(cs));
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(cs));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("uhs", policy => policy.RequireClaim("uh", "LIS"));
    options.AddPolicy("adminsOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddScoped<AuthenticationStateProvider, 
    IdentityValidationProvider<IdentityUser>>();

builder.Services.AddMudServices();

builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
