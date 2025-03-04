﻿using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Contract.Service.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
        string? ValidateToken(string token);
    }
}
