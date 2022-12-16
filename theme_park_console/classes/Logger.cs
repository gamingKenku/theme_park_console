using System;
using System.IO;

namespace theme_park_console
{
    public class Logger
    {
        public event Action<string> LogEvent;
        public void InvokeLogEvent(string message)
        {
            LogEvent?.Invoke(message);
        }
    }
    static class LoggerMethods
    {
        static public void LogInFile(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Config.Log_Path, true))
                {
                    writer.WriteLine($"{DateTime.Now} >> {message}");
                    writer.WriteLine("-----------------------------------------");
                }
            }
            catch
            {
                throw new FileNotFoundException();
            }

        }
        static public void LogInConsole(string message)
        {
            Console.WriteLine($"{DateTime.Now} >> {message}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
