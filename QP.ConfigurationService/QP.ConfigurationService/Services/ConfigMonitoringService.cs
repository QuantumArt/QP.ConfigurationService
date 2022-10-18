using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QP.ConfigurationService.Services
{
    public class ConfigMonitoringService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private IQpConfigurationService _service;
        private int _timeout;

        public ConfigMonitoringService(ILogger<ConfigMonitoringService> logger, IQpConfigurationService service, IConfiguration config)
        {
            _logger = logger;
            _service = service;
            _timeout = config.GetValue("ConfigMonitoringTimeout", 30);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Config Monitoring Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(_timeout));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _service.UpdateConfigurations();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Config Monitoring Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}