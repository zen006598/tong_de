using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using tongDe.Data;
using Vite.AspNetCore.Extensions;
using Serilog;
using Microsoft.AspNetCore.Identity;
using tongDe.Models;
using tongDe.Data.Repository;

var builder = WebApplication.CreateBuilder(args);
Log.Information("Starting web host");
//log config from appsettings.json
Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .CreateLogger();

try
{
    builder.Services.AddControllersWithViews();
    //vite
    builder.Services.AddViteServices();
    //db connection
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"),
        options => options.EnableRetryOnFailure(//暫時性錯誤重試
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
        )));

    //identity user
    builder.Services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOptions"));

    //auto mapper object setting
    builder.Services.AddAutoMapper(typeof(Program));
    //vite run the server after the dotnet run / watch
    builder.Services.AddViteServices(options =>
    {
        options.Server.AutoRun = true;
    });
    //repository
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
    builder.Services.AddScoped<IShopRepository, ShopRepository>();
    builder.Services.AddScoped<IItemRepository, ItemRepository>();
    builder.Services.AddScoped<IItemAliasRepository, ItemAliasRepository>();
    builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

    //log
    builder.Host.UseSerilog();

    var app = builder.Build();
    if (!app.Environment.IsDevelopment())
    {
        //read the static frontend file from wwwroot and root
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
    //request log
    app.UseSerilogRequestLogging();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    if (app.Environment.IsDevelopment())
    {
        app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
            string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
    }

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