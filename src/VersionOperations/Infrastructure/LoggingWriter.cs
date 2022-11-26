using Microsoft.Extensions.Logging;

namespace CompareVersions.Infrastructure;

/// <summary>
///     Logging writer,
/// </summary>
/// <seealso cref="CompareVersions.Interfaces.IMessageWriter" />
public class LoggingWriter : IMessageWriter
{
    private ILogger<LoggingWriter> Logger { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is output redirected.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is output redirected; otherwise, <c>false</c>.
    /// </value>
    public bool IsOutputRedirected => false;

    /// <summary>
    /// Gets a value indicating whether this instance is error redirected.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is error redirected; otherwise, <c>false</c>.
    /// </value>
    public bool IsErrorRedirected => false;

    /// <summary>
    /// Gets a value indicating whether this instance is input redirected.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is input redirected; otherwise, <c>false</c>.
    /// </value>
    public bool IsInputRedirected => false;

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <value>
    /// The error.
    /// </value>
    public IStandardStreamWriter Error { get; }

    /// <summary>
    /// Gets the out.
    /// </summary>
    /// <value>
    /// The out.
    /// </value>
    public IStandardStreamWriter Out { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingWriter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public LoggingWriter(ILogger<LoggingWriter> logger)
        : this(logger, StandardStreamWriter.Create(Console.Out), StandardStreamWriter.Create(Console.Error))
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingWriter"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="stdOut">The standard out.</param>
    /// <param name="stdError">The standard error.</param>
    public LoggingWriter(ILogger<LoggingWriter> logger, IStandardStreamWriter? stdOut = null, IStandardStreamWriter? stdError = null)
    {
        this.Logger = logger;
        this.Out = stdOut ?? StandardStreamWriter.Create(Console.Out);
        this.Error = stdError ?? StandardStreamWriter.Create(Console.Error);
    }

    /// <summary>
    /// Writes the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Write(string? message) => Logger.LogInformation(message);

    /// <summary>
    /// Reads the line.
    /// </summary>
    /// <returns></returns>
    public string? ReadLine()
    {
        return string.Empty;    // DO NOTHING FOR NOW, COULD USE TO SEARCH LOGS/ETC
    }

    /// <summary>
    /// Writes the line.
    /// </summary>
    /// <param name="message">The message.</param>
    public void WriteLine(string? message)
    {
        Logger.LogTrace(message);
    }

    /// <summary>
    /// Reads this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public int Read()
    {
        throw new NotImplementedException();
    }
}
