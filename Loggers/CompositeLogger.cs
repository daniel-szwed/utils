using System.Collections.Generic;
using System.Linq;

namespace ArbreSoft.Utils.Loggers
{
    public class CompositeLogger : Logger
    {
        public CompositeLogger(IList<Logger> loggers)
        {
            childrens = loggers;
        }

        public override bool IsComposite()
        {
            return true;
        }

        public override void Log(string message)
        {
            childrens.ToList().ForEach(logger => logger.Log(message));
        }
    }
}