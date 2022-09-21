namespace CompareVersions.Infrastructure;

public class ConsoleWriter : IMessageWriter
{
    public void Write(string message)
    {
        Console.WriteLine($"MessageWriter.Write(message: \"{message}\")");
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
