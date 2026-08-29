using System;
using System.IO;
using System.Web;

namespace Payroll_HCC.Infrastructure
{
    /// <summary>
    /// Minimal thread-safe daily-rolling file logger (App_Data\logs\app-yyyyMMdd.log).
    /// No external dependencies; sufficient until a full logging framework is adopted.
    /// </summary>
    public static class FileLogger
    {
        static readonly object Gate = new object();

        public static void Error(string message, Exception ex = null)
        {
            Write("ERROR", message, ex);
        }

        public static void Info(string message)
        {
            Write("INFO", message, null);
        }

        static void Write(string level, string message, Exception ex)
        {
            try
            {
                string dir = HttpRuntime.AppDomainAppPath != null
                    ? Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "logs")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(dir);
                string file = Path.Combine(dir, "app-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} [{1}] {2}{3}",
                    DateTime.Now, level, message,
                    ex == null ? "" : Environment.NewLine + ex);
                lock (Gate)
                {
                    File.AppendAllText(file, line + Environment.NewLine);
                }
            }
            catch
            {
                // Logging must never take the app down.
            }
        }
    }
}
