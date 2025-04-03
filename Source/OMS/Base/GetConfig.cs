using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OMS
{
    public class GetConfig
    {
        #region GetConfigValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int getIntValue(string key)
        {
            int intValue = 0;
            if (!int.TryParse(getValue(key), out intValue))
            {
                intValue = 0;
            }
            return intValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getStringValue(string key)
        {
            return getValue(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool getBoolValue(string key)
        {
            bool boolValue;
            if (!bool.TryParse(getValue(key), out boolValue))
            {
                boolValue = false;
            }
            return boolValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string getValue(string key)
        {
            string value = null;

            if (!string.IsNullOrEmpty(key))
            {
                value = ConfigurationManager.AppSettings[key];
            }

            return value;
        }
        #endregion
    }
}