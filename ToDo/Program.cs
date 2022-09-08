using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDo.Captcha;
using ToDo.Data;
using ToDoEntityFramework;
using Microsoft.AspNetCore.Identity;
using ToDo.Areas.Identity.Data;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");


builder.Host.UseSerilog(Log.Logger);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlite("Data Source = ToDoData.db");
});

builder.Services.AddDefaultIdentity<ToDoUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddScoped<ITodoRepository, TodoEFRepository>();
builder.Services.AddHttpClient<ReCaptchaValidator>();
builder.Services.AddScoped<ICaptchaValidator, ReCaptchaValidator>();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseHttpLogging();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Captcha}/{id?}");
app.MapRazorPages();
app.Run();

