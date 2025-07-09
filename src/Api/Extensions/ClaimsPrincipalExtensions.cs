using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Foydalanuvchi aniqlanmadi.");

        return long.Parse(userIdClaim);
    }
}