// See https://aka.ms/new-console-template for more information
using Version = CompareVersions.Models.Version;

namespace CompareVersions;
/// <summary>
/// Main entry class
/// </summary>
public static class Program
{
    /// <summary>
    /// Mains the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public static async Task<int> Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
                services
                    .AddTransient<IComparisonOperations<Version>, VersionComparer<Version>>()
                    .AddHostedService<Worker>()
                    .AddTransient<IMessageWriter, ConsoleWriter>()
                    // .AddTransient<IMessageWriter, LoggingWriter>() // Removed to avoid ambiguity
                    .AddTransient<ICommandLine, CommandLineOptions>()
                    .AddScoped<Version, Version>()
                    .AddScoped<Application<Version>>())
            .Build();

        var application = host.Services.GetRequiredService<Application<Version>>();
        return await application.RunAsync(args);

    }

    /// <summary>
    /// Handle the exception trapper.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
    private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
    {
        Console.WriteLine(e.ExceptionObject.ToString());
        Console.WriteLine("Press Enter to Exit");
        Console.ReadLine();
        Environment.Exit(0);
    }
}