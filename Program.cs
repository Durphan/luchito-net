using System.Reflection;
using luchito_net.Config;
using luchito_net.Config.DataProvider;
using luchito_net.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

string connectionString = builder.Configuration.GetConnectionString("DockerPostgreSql") ?? throw new Exception("Connection string 'DockerPostgreSql' not found.");


builder.Services.AddRazorPages();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.RazorPages.RazorPagesOptions>(options =>
{
    options.RootDirectory = "/view/Pages"; ;

});

Inject.ConfigureServices(builder.Services);
builder.Services.TryAddTransient<Middleware>();

builder.Services.AddDbContext<InitializeDatabase>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Middleware>();
});


var app = builder.Build();

app.UseMiddleware<Middleware>();

IServiceScope scope = app.Services.CreateScope();
{
    InitializeDatabase dbContext = scope.ServiceProvider.GetRequiredService<InitializeDatabase>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "luchito-net API V1");
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}




app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.MapStaticAssets();
app.MapRazorPages();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "view", "wwwroot")),
    RequestPath = "",
});

await app.RunAsync();
