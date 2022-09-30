namespace CompareVersions.Infrastructure;

/// <summary>
///     Represents an <see cref="IMessageWriter"/> object that writes to the standard output stream
/// </summary>
/// <seealso cref="CompareVersions.Interfaces.IMessageWriter" />
public class ConsoleWriter : IMessageWriter
{
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
    /// Initializes a new instance of the <see cref="ConsoleWriter"/> class.
    /// </summary>
    /// <param name="stdOut">The standard out.</param>
    /// <param name="stdError">The standard error.</param>
    public ConsoleWriter(IStandardStreamWriter stdOut = null, IStandardStreamWriter stdError = null)
    {
        this.Out = stdOut ?? StandardStreamWriter.Create(Console.Out);
        this.Error = stdError ?? StandardStreamWriter.Create(Console.Error);
    }

    /// <summary>
    /// Writes the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Write(string? message) => Console.Write(message);

    /// <summary>
    /// Writes the line.
    /// </summary>
    /// <param name="message">The message.</param>
    public void WriteLine(string? message) => Console.WriteLine(message);

    /// <summary>
    /// Reads this instance.
    /// </summary>
    /// <returns>A <see cref="System.String"/>from the standard IO stream.</returns>
    public string? ReadLine() => Console.ReadLine();

    /// <summary>
    /// Reads this instance.
    /// </summary>
    /// <returns></returns>
    public int Read() => Console.Read();
}
