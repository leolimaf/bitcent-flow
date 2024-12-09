namespace BitcentFlow.Auth.DTOs.UserDTOs.Requests;

public record TokenRequest(string AccessToken, string RefreshToken);