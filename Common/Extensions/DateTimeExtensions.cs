using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new NullReferenceException();
            }

            CultureInfo CultureInfo = new CultureInfo("en-GB");
      
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
    }
    public enum UnixConversionOptions
    {
        seconds,
        milliseconds
    }
}
