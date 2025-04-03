using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MailProcess
{
    public class GetConfig
    {
        #region GetConfigValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int getIntValue(string value)
        {
            int intValue;
            if (!int.TryParse(value, out intValue))
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
        public static bool getBoolValue(string value)
        {
            bool boolValue;
            if (!bool.TryParse(value, out boolValue))
            {
                boolValue = false;
            }
            return boolValue;
        }

        #endregion
    }
}
