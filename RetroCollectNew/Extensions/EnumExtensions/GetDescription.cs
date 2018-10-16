using System;
using System.ComponentModel;

namespace RetroCollectNew.Extensions.EnumExtensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum x)
        {
            var attributes = x.GetType().GetField(x.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0 ? x.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
