
using API.Middleware;

using Core.Abstractions;

using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.SeedData;

using Microsoft.EntityFrameworkCore;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddCors();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseCors(x => x.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200")
                  );

        app.MapControllers();

        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            await dbContext.Database.MigrateAsync();

            await StoreDbContextSeed.SeedAsync(dbContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        app.Run();
    }
}
