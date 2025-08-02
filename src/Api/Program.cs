using Api.Configurations;
using Api.Endpoints;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http.Features;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.ListenAnyIP(5000); // Hamma IP-lardan 5000-portda qabul qiladi
        //});

        builder.Configuration.AddJsonFile("appsettings.json");
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

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 524288000; // 500 MB
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });

            // JWT Bearer token qo‘llab-quvvatlashi uchun
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT tokenni quyidagicha kiriting: `Bearer {token}`",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });



        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost5173", policy =>
            {
                policy.WithOrigins(
                    "http://localhost:4200",
                    "http://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        var cloudinaryAccount = new Account(
            builder.Configuration["Cloudinary:CloudName"],
            builder.Configuration["Cloudinary:ApiKey"],
            builder.Configuration["Cloudinary:ApiSecret"]
        );

        var cloudinary = new Cloudinary(cloudinaryAccount);

        // ?? DI ga qo‘shamiz
        builder.Services.AddSingleton(cloudinary);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseCors("AllowLocalhost5173");
        app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();
        app.MapChannelEndpoints();
        app.MapVideoEndpoints();
        app.MapCommentEndpoints();
        app.MapLikeDislikeEndpoints();
        app.MapSubscriptionEndpoints();
        app.MapAIEndpoints();

        app.MapControllers();

        app.Run();
    }
}
