using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using tongDe.Data;
using Vite.AspNetCore.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .CreateLogger();

Log.Information("Starting web host");

try
{
    builder.Services.AddControllersWithViews();

    builder.Services.AddViteServices();

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));

    builder.Services.AddViteServices(options =>
    {
        options.Server.AutoRun = true;
    });

    builder.Host.UseSerilog();

    var app = builder.Build();
    if (!app.Environment.IsDevelopment())
    {
        var webRootProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot"));
        var distProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "dist"));
        var compositeProvider = new CompositeFileProvider(webRootProvider, distProvider);
        app.Environment.WebRootFileProvider = compositeProvider;
        app.Environment.WebRootPath = distProvider.Root;
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        app.UseViteDevelopmentServer(true);
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();
    
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}