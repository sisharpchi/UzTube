using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations;

public static class DataBaseConfiguration
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    }
}
