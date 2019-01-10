using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QP.ConfigurationService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Services
{
    public interface ICustomersConfigurationsService
    {
        CustomerConfiguration GetCustomerConfig(string customerName);
        ICollection<string> GetCustomersNames();
    }

    public class CustomersConfigurationsService : ICustomersConfigurationsService
    {
        const string ConfigFilePathKey = "QpConfigurationPath";

        readonly ILogger<CustomersConfigurationsService> _logger;

        string _configFilePath;
        FileSystemWatcher _configFileWatcher;
        XmlSerializer _configSerializer = new XmlSerializer(typeof(Configuration));
        Dictionary<string, CustomerConfiguration> _customersConfigurations = new Dictionary<string, CustomerConfiguration>();

        public CustomersConfigurationsService(
            IConfiguration configuration,
            ILogger<CustomersConfigurationsService> logger)
        {
            _logger = logger;
            _configFilePath = configuration.GetValue<string>(ConfigFilePathKey);

            if (String.IsNullOrWhiteSpace(_configFilePath) || !File.Exists(_configFilePath))
                return;

            SubscribeOnConfigChanges();
            UpdateConfigurations();
        }

        public CustomerConfiguration GetCustomerConfig(string customerName)
        {
            return _customersConfigurations.GetValueOrDefault(customerName);
        }

        public ICollection<string> GetCustomersNames()
        {
            return _customersConfigurations.Keys;
        }

        void UpdateConfigurations()
        {
            try
            {
                using (var xmlTextReader = new XmlTextReader(_configFilePath))
                {
                    var config = (Configuration)_configSerializer.Deserialize(xmlTextReader);
                    _customersConfigurations = config.Customers.ToDictionary(c => c.Name, c => c);
                }
                _logger.LogInformation("Configuration successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update configuration");
                _customersConfigurations = new Dictionary<string, CustomerConfiguration>();
            }
        }

        void SubscribeOnConfigChanges()
        {
            try
            {
                _configFileWatcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(_configFilePath),
                    Filter = Path.GetFileName(_configFilePath)
                };

                _configFileWatcher.Changed +=
                    (object sender, FileSystemEventArgs e) => { UpdateConfigurations(); };
                _configFileWatcher.EnableRaisingEvents = true;

                _logger.LogInformation($"Subscribed on config file updates: {_configFilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe on config updates");
            }
        }
    }
}
