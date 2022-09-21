// See https://aka.ms/new-console-template for more information
using CompareVersions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            .AddScoped<IComparisonOperations<CompareVersions.UI.Version>, VersionComparer<CompareVersions.UI.Version>>()
            .AddHostedService<Worker>()
            .AddSingleton<IMessageWriter, ConsoleWriter>()
            .AddSingleton<IMessageWriter, LoggingWriter>()
            .AddScoped<Harness<CompareVersions.UI.Version>>())
    .Build();


try
{
    var harness = host.Services.GetRequiredService<Harness<CompareVersions.UI.Version>>();
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
