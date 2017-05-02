using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.DotGIS.Common
{
    public enum LogLevel
    {
        Common,
        Error
    }
    public static class Log
    {
        private static log4net.ILog _runtimeLogger;
        private static log4net.ILog _runtimeErrorLogger;
        static Log()
        {
            log4net.Config.XmlConfigurator.Configure();
            _runtimeLogger = log4net.LogManager.GetLogger("loginfo");
            _runtimeErrorLogger = log4net.LogManager.GetLogger("logerror");
        }
        public static void WriteLog(string content, LogLevel level)
        {
            if (level == LogLevel.Common)
            {
                Log._runtimeLogger.Info(content);
            }
            else
            {
                Log._runtimeErrorLogger.Error(content);
            }
        }

    }
}
