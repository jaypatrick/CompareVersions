using Microsoft.Extensions.Logging;

namespace CompareVersions.Infrastructure;


/// <summary>
/// Background service worker class for logging
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public Worker(ILogger<Worker> logger) => _logger = logger;


    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
    /// </remarks>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);
            //await Task.Delay(1000, stoppingToken);

            // THIS IS WHERE YOU WOULD LOG TWITTER API CALLS
        }
    }
}