namespace CompareVersions.Interfaces
{
    public interface IMessageWriter
    {
        void Write(string message);

        string? Read();
    }
}
