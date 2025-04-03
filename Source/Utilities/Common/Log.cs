using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Utilities
{
    public enum LogType
    {
        Debug = 0,
        Fatal = 1,
        Error = 2
    }

    public interface ILog
    {
        void WriteLog(string log, LogType type = LogType.Debug);
        void WriteLog(Exception ex);
    }

    /// <summary>
    /// Log Utility class
    /// Author: ISV-Phuong
    /// </summary>
    public sealed class Log : ILog
    {
        static readonly ILog _instance = new Log();
        public static ILog Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Write Log
        /// Author: ISV-Phuong
        /// </summary>
        /// <param name="ex">Exception</param>
        public void WriteLog(string log, LogType type = LogType.Debug)
        {

            ////GET logger
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //Write log
            switch (type)
            {
                case LogType.Debug:
                    logger.Debug(log.ToString());
                    break;
                case LogType.Fatal:
                    logger.Fatal(log.ToString());
                    break;
                case LogType.Error:
                    logger.Error(log.ToString());
                    break;
            }
        }

        /// <summary>
        /// Write Log
        /// Author: ISV-Phuong
        /// </summary>
        /// <param name="ex">Exception</param>
        public void WriteLog(Exception ex)
        {
            //Error content
            StringBuilder builder = new StringBuilder(Environment.NewLine);
            builder.AppendLine()
                    .AppendLine("--------------------------------")
                    .AppendFormat("Message:\t{0}", ex.Message)
                    .AppendLine()
                    .AppendFormat("Error Info:\t{0}", ex)
                    .AppendLine()
                    .AppendLine()
                    .AppendFormat("Source:\t{0}", ex.Source)
                    .AppendLine()
                    .AppendFormat("Target:\t{0}", ex.TargetSite)
                    .AppendLine()
                    .AppendFormat("Type:\t{0}", ex.GetType().Name)
                    .AppendLine()
                    .AppendFormat("Stack:\t{0}", ex.StackTrace);
            if (ex.InnerException != null)
            {
                builder.AppendLine()
                        .AppendFormat("InnerException:\t{0}", ex.InnerException.Message);
            }
            builder.AppendLine()
                    .AppendLine("--------------------------------")
                    .AppendLine();

            WriteLog(builder.ToString(), LogType.Error);
        }
    }
}
