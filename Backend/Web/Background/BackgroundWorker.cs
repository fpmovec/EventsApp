using Application.Services;
using System.Timers;

namespace Web.Background
{
    public class BackgroundWorker : IHostedService, IDisposable
    {
        private const long WorkDelayInMiliseconds = 86_400_000 ;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundWorker> _logger;
        private System.Timers.Timer _timer;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _timer = new(WorkDelayInMiliseconds);

            _timer.Elapsed += async (source, ev) => await DoWorkAsync(source, ev);
            _serviceProvider = serviceProvider;
        }

        private async Task DoWorkAsync(Object source, ElapsedEventArgs e)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IJwtService jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

            await jwtService.ClearUnusedRefreshTokensAsync(default);

            _logger.LogInformation("Expired refresh tokens have been removed");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _logger.LogInformation("Timed Hosted Service running.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Enabled = false;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
