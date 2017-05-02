using System;
using Demo.DotGIS.Common;

namespace Demo.DotGIS.Util
{
    public class MessageConsole
    {
        static Common.Delegate.OnLogger _onLogger = null;
        public static void Write(string msg, LogLevel level = LogLevel.Common)
        {
            if (MessageConsole._onLogger != null)
            {
                MessageConsole._onLogger(msg, level);
            }
            else
            {
                DateTime dtime = DateTime.Now;
                string txt = "[" + dtime + "] " + msg;
                Common.Log.WriteLog(txt, level);
            }
        }
        public static void Write(string msg)
        {
            if (MessageConsole._onLogger != null)
            {
                MessageConsole._onLogger(msg, LogLevel.Common);
            }
            else
            {
                DateTime dtime = DateTime.Now;
                string txt = "[" + dtime + "] " + msg;
                Common.Log.WriteLog(txt, LogLevel.Common);
            }
        }
        public static void Write(Exception ex)
        {
            if (MessageConsole._onLogger != null)
            {
                MessageConsole._onLogger(ex.Message + "\r\n\t" + ex.StackTrace, LogLevel.Error);
            }
            else
            {
                Log.WriteLog(ex.Message + "\r\n\t" + ex.StackTrace, LogLevel.Error);
            }
        }
    }
}
