using ApplicationLayer.Extensions.EnumExtensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ApplicationLayer.Helpers
{
    public class EnumerationsHelper
    {
        public static IEnumerable GetDescriptionList<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            List<string> x = new List<string>();

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var y = value as Enum;
                x.Add(y.GetDescription());
            }

            return x;
        }
    }
}
