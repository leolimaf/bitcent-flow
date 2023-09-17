﻿namespace MinhasFinancas.Auth.Configurations;

public class TokenConfiguration
{
    public string Secret { get; set; }
    public int Minutes { get; set; }
    public int DaysToExpiry { get; set; }
}