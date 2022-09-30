namespace CompareVersions.Interfaces
{
    /// <summary>
    ///     Interface for writing to the standard console
    /// </summary>
    /// <seealso cref="System.CommandLine.IConsole" />
    public interface IMessageWriter : IConsole
    {
        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Write(string? message);

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        void WriteLine(string? message);

        /// <summary>
        /// Reads the line.
        /// </summary>
        /// <returns></returns>
        string? ReadLine();

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        int Read();
    }
}
