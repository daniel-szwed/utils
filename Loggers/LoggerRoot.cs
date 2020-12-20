using System.Collections.Generic;

namespace ArbreSoft.Utils.Loggers
{
    public class LoggerRoot : CompositeLogger
    {
        public LoggerRoot(IList<Logger> loggers) : base(loggers) { }
    }
}