using Prism.Logging;

namespace ElectroneumSpace.Services
{
    public class LoggerFacade : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority) => System.Diagnostics.Debug.WriteLine($"[{priority}] - [{category}] - {message}");
    }
}
