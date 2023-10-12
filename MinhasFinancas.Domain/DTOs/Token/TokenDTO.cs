﻿namespace MinhasFinancas.Domain.DTOs.Token;

public class TokenDTO
{
    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Message { get; set; }

    public TokenDTO()
    {
    }

    public TokenDTO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
    {
        Authenticated = authenticated;
        Created = created;
        Expiration = expiration;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}