using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string inputString, string culture = "en-GB")
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new NullReferenceException();
            }

            CultureInfo CultureInfo = new CultureInfo(culture);
      
            return DateTime.Parse(inputString, CultureInfo);       
        }

        public static long ToUnix(this DateTime dateTime, UnixConversionOptions conversionOption = UnixConversionOptions.milliseconds)
        {
            DateTimeOffset x = new DateTimeOffset(dateTime);

            switch (conversionOption)
            {
                case UnixConversionOptions.seconds:
                    return x.ToUnixTimeSeconds();
                case UnixConversionOptions.milliseconds:
                    return x.ToUnixTimeMilliseconds();
            }

            throw new Exception("Could not find conversion option");
        }

        public static string FromUnix(this long dateStamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(dateStamp).ToString("dd/MM/yyyy");
        }

        public static string FromUnix(this string dateStamp)
        {
            if (string.IsNullOrEmpty(dateStamp)) return null;

            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(dateStamp)).ToString("dd/MM/yyyy");
        }



    }
    public enum UnixConversionOptions
    {
        seconds,
        milliseconds
    }
}
