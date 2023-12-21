namespace MyFinances.Application.Services.Authentication.Common.Requests;

public record AtualizacaoTokenRequest(
    string AccessToken,
    string RefreshToken
);