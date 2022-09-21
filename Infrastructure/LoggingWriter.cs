using Microsoft.Extensions.Logging;

namespace CompareVersions.Infrastructure;

public class LoggingWriter : IMessageWriter
{
    private readonly ILogger<LoggingWriter> _logger;

    public LoggingWriter(ILogger<LoggingWriter> logger) => _logger = logger;

    public void Write(string message) => _logger.LogInformation(message);

    public string? Read()
    {
        return string.Empty;    // DO NOTHING FOR NOW, COULD USE TO SEARCH LOGS/ETC
    }
}
