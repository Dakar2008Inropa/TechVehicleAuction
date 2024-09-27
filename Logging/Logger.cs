using log4net;
using log4net.Config;

namespace Logging
{
    public static class Logger
    {
        static Logger()
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}