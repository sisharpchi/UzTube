﻿using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IRefreshTokenRepository
{
    Task AddRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId);
    Task DeleteRefreshToken(string refreshToken);
}