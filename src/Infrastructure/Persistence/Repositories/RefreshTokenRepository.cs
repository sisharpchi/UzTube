using Application.Contracts.Repository;
using Core.Errors;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(AuthDbContext authDbContext) : IRefreshTokenRepository
{
    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await authDbContext.RefreshTokens.AddAsync(refreshToken);
        await authDbContext.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        var token = await authDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (token == null)
        {
            throw new EntityNotFoundException();
        }
        authDbContext.RefreshTokens.Remove(token);
    }

    public async Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId) => await authDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
}