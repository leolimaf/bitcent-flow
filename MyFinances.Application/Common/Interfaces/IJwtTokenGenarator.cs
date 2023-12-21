﻿using System.Security.Claims;
using MyFinances.Application.Services.Authentication.Common.Responses;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Common.Interfaces;

public interface IJwtTokenGenarator
{
    LoginUsuarioResponse GerarToken(Usuario usuario);
    string GerarAccessToken(IEnumerable<Claim> claims);
    string GerarRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    LoginUsuarioResponse RetornarTokenAtualizado(string accessToken, string refreshToken);
}