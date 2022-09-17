using System;

namespace ArbreSoft.Utils
{
    public static class TimeUtil
    {
        public static DateTime UtcNow()
        {
            var n = DateTime.UtcNow;
            return new DateTime(n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second, DateTimeKind.Utc);
        }
    }
}