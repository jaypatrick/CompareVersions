// See https://aka.ms/new-console-template for more information
using CompareVersions;
using Version = CompareVersions.UI.Version;


AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            .AddScoped<IComparisonOperations<Version>, VersionComparer<Version>>()
            .AddHostedService<Worker>()
            .AddSingleton<IMessageWriter, ConsoleWriter>()
            .AddSingleton<IMessageWriter, LoggingWriter>()
            .AddSingleton<ICommandLine, CommandLineOptions>()
            .AddSingleton<IConsole, SystemConsole>()
            .AddScoped<Harness<Version>>())
    .Build();


try
{
    var harness = host.Services.GetRequiredService<Harness<Version>>();
    await harness.RunAsync(args);

    //harness.Run(args, host.Services);
    //host.Run();
}
catch (Exception exc)
{
    Console.WriteLine(exc.ToString());
    Console.WriteLine("Press Enter to Exit");
    Console.ReadLine();
}
finally
{
    Console.WriteLine("Press any key to run this application again, or simply close the app");
    host.Run();
}

static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
{
    Console.WriteLine(e.ExceptionObject.ToString());
    Console.WriteLine("Press Enter to Exit");
    Console.ReadLine();
    Environment.Exit(0);
}
