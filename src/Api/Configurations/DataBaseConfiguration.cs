using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations;

public static class DataBaseConfiguration
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var sqlConnection = builder.Configuration.GetConnectionString("SqlServerConnection");
        var postgresConnection = builder.Configuration.GetConnectionString("PostgresConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlConnection));

        builder.Services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(postgresConnection));
    }
}
