namespace MyFinances.Domain.Authentication.Requests;

public record AtualizacaoTokenRequest(
    string AccessToken,
    string RefreshToken
);