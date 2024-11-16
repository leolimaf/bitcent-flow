namespace BitcentFlow.Infrastructure.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Secret { get; init; }
    public int Minutes { get; init; }
    public int DaysToExpiry { get; init; }
}