using System.Collections.Generic;

namespace ArbreSoft.Utils.Loggers
{
    public class RemoteLogger : CompositeLogger
    {
        public RemoteLogger(IList<Logger> loggers) : base(loggers) { }
    }
}