namespace CompareVersions.Interfaces;

/// <summary>
/// Marker interface to specify operations for a writer that writes to the standard cout on IConsole
/// </summary>
/// <seealso cref="System.CommandLine.IConsole" />
/// <seealso cref="CompareVersions.Interfaces.IMessageWriter" />
public interface IConsoleWriter : IConsole, IMessageWriter
{
}