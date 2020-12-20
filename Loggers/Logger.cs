using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbreSoft.Utils.Loggers
{
    public abstract class Logger
    {
        public virtual IList<Logger> childrens { get; set; }
        
        public virtual void Add(Logger logger)
        {
            if (!IsComposite())
                throw new InvalidOperationException("Add method is not allowed for leaf");
            else
                childrens.Add(logger);
        }

        public abstract void Log(string message);

        public virtual void Log<T>(string message) where T : Logger
        {
            if(typeof(T) == this.GetType())
            {
                Log(message);
            }
            else
            {
                childrens?.ToList().ForEach(logger => logger.Log<T>(message));
            }
        }

        public virtual void Remove<T>() where T : Logger
        {
            if (!IsComposite())
                throw new InvalidOperationException("Remove method is not allowed for leaf");
            else
                childrens.ToList().RemoveAll(logger => logger.GetType() == typeof(T));
        }

        public abstract bool IsComposite();
    }
}