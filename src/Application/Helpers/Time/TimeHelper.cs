﻿namespace Application.Helpers.Time;

public class TimeHelper
{
    public static DateTime GetDateTime()
    {
        var dtTime = DateTime.UtcNow;
        dtTime.AddHours(5);
        return dtTime;
    }
}
