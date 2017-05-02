using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.DotGIS.Common
{
    public class Delegate
    {
        public delegate void OnLogger(object msg, LogLevel level = LogLevel.Common);
        public delegate void TaskCackback(object param);
    }

}
