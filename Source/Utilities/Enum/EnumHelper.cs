using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMS.Utilities
{
    /// <summary>
    /// Enum Helper
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Get Description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            string output = null;
            Type type = enumValue.GetType();
            FieldInfo fi = type.GetField(enumValue.ToString());
            var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attrs.Length > 0) output = attrs[0].Description;
            return output;
        }

    }
}
