using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Common.Extensions.EnumExtensions
{
    public static class EnumHelper
    {
        /// <summary>
        /// Helper method to get the description from enum.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum x)
        {
            var attributes = x.GetType().GetField(x.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0 ? x.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        //TODO: Add 
        private static IEnumerable<string> GetDescriptions(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }


    }


    
}
