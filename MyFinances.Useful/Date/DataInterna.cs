namespace MyFinances.Useful.Date;

public static class DataInterna
{
    public static DateTime ObterHorarioDeBrasilia()
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        TimeZoneInfo timeZoneBrasilia = null!;
        foreach (var timeZone in timeZones)
            if ((int)timeZone.BaseUtcOffset.TotalMinutes == -180)
                timeZoneBrasilia =  timeZone;
        
        return TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneBrasilia);
    }
}