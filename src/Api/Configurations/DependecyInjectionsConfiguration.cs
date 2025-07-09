using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Contracts.SeviceContracts;
using Application.Dtos.User;
using Application.Helpers.Token;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Persistence.Repositories;

namespace Api.Configurations;

public static class DependecyInjectionsConfiguration
{
    public static void ConfigureDependecies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
        builder.Services.AddScoped<IVideoRepository, VideoRepository>();
        builder.Services.AddScoped<IViewHistoryRepository, ViewHistoryRepository>();
        builder.Services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        builder.Services.AddScoped<IUploadService, VideoService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IVideoService, VideoService>();
        builder.Services.AddScoped<IChannelService, ChanelService>();
        builder.Services.AddScoped<ICommentService, CommentService>();

        builder.Services.AddScoped<IValidator<UserRegisterDto>, UserCreateDtoValidator>();
        builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
    }
}