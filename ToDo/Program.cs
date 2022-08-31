using Serilog;
using ToDo.Captcha;
using ToDo.Data;
var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog(Log.Logger);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Captcha}/{id?}");

app.Run();
