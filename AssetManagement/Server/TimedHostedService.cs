using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Server;
//using AssetManagement.Server.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TimedHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly BackgroundServiceOptions _options;
    private Timer _timer;
    //private readonly OnboardingConfirmation _onboardingConfirmation;

    public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory, IOptions<BackgroundServiceOptions> options )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        //_onboardingConfirmation = onboardingConfirmation;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var runAt = DateTime.ParseExact(_options.RunAt, "HH:mm", CultureInfo.InvariantCulture);
        var firstRun = new DateTime(now.Year, now.Month, now.Day, runAt.Hour, runAt.Minute, 0);

        if (now > firstRun)
        {
            firstRun = firstRun.AddDays(1);
        }

        var initialDelay = (firstRun - now).TotalMilliseconds;
        var interval = TimeSpan.FromDays(1).TotalMilliseconds;

        _timer = new Timer(DoWork, null, (long)initialDelay, (long)interval);

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Timed Background Service is working.");

        using (var scope = _scopeFactory.CreateScope())
        {
            var sharePointService = scope.ServiceProvider.GetRequiredService<SharePointService>();
            
        }
        //_onboardingConfirmation.OnBoardingquery();

        
    }



    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
