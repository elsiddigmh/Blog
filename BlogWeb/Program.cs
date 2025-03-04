using BlogWeb;
using BlogWeb.Services;
using BlogWeb.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Mapping
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Services
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddHttpClient<IPostService, PostService>();
builder.Services.AddScoped<IPostService, PostService>();



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	options.LoginPath = "/Login";
	options.AccessDeniedPath = "/AccessDenied";
	options.SlidingExpiration = true;
});
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(100);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Configure Serilog to logging in file 
Log.Logger = new LoggerConfiguration().WriteTo.Console()
									  .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();


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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
