namespace CompareVersions.Infrastructure;

public class ConsoleWriter : IMessageWriter
{
    private IConsole _standardConsole { get; }

    public ConsoleWriter(IConsole standardConsole)
    {
        _standardConsole = standardConsole;
    }

    public void Write(string message)
    {
        this._standardConsole.WriteLine($"MessageWriter.Write(message: \"{message}\")");
    }

    /// <summary>
    /// Reads this instance.
    /// </summary>
    /// <returns>A <see cref="System.String"/>from the standard IO stream.</returns>
    public string? Read()
    {
        return Console.ReadLine();
    }
}
