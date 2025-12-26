using Api.Extensions;
using Application.Contracts.SeviceContracts;
using Application.Dtos.User;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        userGroup.MapPost("/send-code",
            async (string email, IAuthService _service) =>
            {
                await _service.EailCodeSender(email);
            })
            .WithName("SendCode");

        userGroup.MapPost("/confirm-code",
            async (string code, string email, IAuthService _service) =>
            {
                var res = await _service.ConfirmCode(code, email);
                return Results.Ok(res);
            })
            .WithName("ConfirmCode");

        userGroup.MapPost("/sign-up",
        async (UserRegisterDto user, IAuthService _service) =>
        {
            return Results.Ok(await _service.SignUpUserAsync(user));
        })
        .WithName("SignUp");

        userGroup.MapPost("/login",
        async (UserLoginDto user, IAuthService _service) =>
        {
            return Results.Ok(await _service.LoginUserAsync(user));
        })
        .WithName("Login");

        userGroup.MapPut("/refresh-token",
        async (RefreshRequestDto refresh, IAuthService _service) =>
        {
            return Results.Ok(await _service.RefreshTokenAsync(refresh));
        })
        .WithName("RefreshToken");

        userGroup.MapDelete("/log-out",
        async (string refreshToken, IAuthService _service) =>
        {
            await _service.LogOut(refreshToken);
            return Results.Ok();
        })
        .WithName("LogOut");

        userGroup.MapPut("/change-password",
        async (HttpContext context, UserChangePasswordDto dto, IAuthService _service) =>
        {
            long userId = context.User.GetUserId();
            var result = await _service.ChangePasswordAsync(dto, userId);

            if (!result)
                return Results.BadRequest(new { message = "Old password is incorrect." });

            return Results.Ok(new { message = "Password changed successfully." });
        })
        .RequireAuthorization()
        .WithName("ChangePassword");

        userGroup.MapGet("/profile",
        async (HttpContext context ,IAuthService _service) =>
        {
            long userId = context.User.GetUserId();
            var user = await _service.GetMeAsync(userId);
            return Results.Ok(new { data = user });
        })
        .RequireAuthorization()
        .WithName("GetProfile");

        userGroup.MapPut("/edit-profile",
        async (HttpContext context, string fullName, IAuthService _service) =>
        {
            long userId = context.User.GetUserId();
            await _service.EditMeAsync(userId, fullName);
            return Results.Ok(new { message = "Profile updated successfully." });
        })
        .RequireAuthorization()
        .WithName("EditProfile");
    }
}
