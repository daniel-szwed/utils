using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace ArbreSoft.Utils.Loggers
{
    public class FileLogger : Logger
    {
        private readonly object _lock = new object();
        private string name;
        private string dir;
        private string Today => DateTime.Now.ToString("yyyy-MM-dd");
        private string Filename => $"[{Today}]_{name}";
        private string Path => @$"{dir}/{Filename}.log";

        public FileLogger(IOptions<FileLoggerConfig> config)
        {
            name = Assembly.GetCallingAssembly().GetName().Name;
            dir = config.Value.Dir;
            Init();
        }

        private void Init()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public override void Log(string message)
        {
            lock(_lock)
            {
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public override bool IsComposite()
        {
            return false;
        }
    }
}