using System.Collections.Generic;

namespace ArbreSoft.Utils.Loggers
{
    public class LocalLogger : CompositeLogger
    {
        public LocalLogger(IList<Logger> loggers) : base(loggers) {}
    }
}