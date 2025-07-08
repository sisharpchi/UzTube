using Api.Configurations;
using Api.Endpoints;
using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Services;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http.Features;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.ConfigureSerilog();
        builder.ConfigureDatabase();
        builder.ConfigurationJwtAuth();
        builder.ConfigureJwtSettings();
        //builder.ConfigureSerilog();
        builder.ConfigureDependecies();

        builder.Services.AddMegaIntegration(builder.Configuration);
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500 MB
        });
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500 MB
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();
        app.MapUploadEndpoints();
        app.MapControllers();

        app.Run();
    }
}
