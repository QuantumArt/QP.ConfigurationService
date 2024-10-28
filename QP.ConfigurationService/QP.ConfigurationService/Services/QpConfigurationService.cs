using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QP.ConfigurationService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Services
{
    public interface IQpConfigurationService
    {
        CustomerConfiguration GetCustomerConfig(string customerName);
        ICollection<CustomerConfiguration> GetCustomersConfigs();
        ICollection<ApplicationVariable> GetVariables();

        void UpdateConfigurations();
    }

    public class QpConfigurationService : IQpConfigurationService
    {
        const string ConfigFilePathKey = "QpConfigurationPath";

        readonly ILogger<QpConfigurationService> _logger;

        string _configFilePath;
        private DateTime _lastWriteTime;
        XmlSerializer _configSerializer = new XmlSerializer(typeof(Configuration));
        
        Dictionary<string, CustomerConfiguration> _customersConfigurations = new Dictionary<string, CustomerConfiguration>();
        Dictionary<string, ApplicationVariable> _variables = new Dictionary<string, ApplicationVariable>();

        public QpConfigurationService(
            IConfiguration configuration,
            ILogger<QpConfigurationService> logger)
        {
            _logger = logger;
            _configFilePath = configuration.GetValue<string>(ConfigFilePathKey);

            if (String.IsNullOrWhiteSpace(_configFilePath) || !File.Exists(_configFilePath))
                return;

            UpdateConfigurations();
        }

        public CustomerConfiguration GetCustomerConfig(string customerName)
        {
            return _customersConfigurations.GetValueOrDefault(customerName);
        }

        public ICollection<CustomerConfiguration> GetCustomersConfigs()
        {
            return _customersConfigurations.Values;
        }

        public ICollection<ApplicationVariable> GetVariables()
        {
            return _variables.Values;
        }
        
        public void UpdateConfigurations()
        {
            try
            {
                FileInfo fi = new FileInfo(_configFilePath);
                if (_lastWriteTime == fi.LastWriteTime)
                {
                    _logger.LogInformation("Configuration not changed: " + fi.LastWriteTime);               
                }
                else
                {

                    using (StreamReader stream = new StreamReader(_configFilePath))
                    {
                        var config = (Configuration) _configSerializer.Deserialize(stream);
                        _customersConfigurations = config.Customers.ToDictionary(c => c.Name, c => c);
                        _variables = config.Variables.ToDictionary(v => v.Name, v => v);
                    }
                    _lastWriteTime = fi.LastWriteTime;
                    _logger.LogInformation("Configuration successfully updated: " + _lastWriteTime);                 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update configuration");
            }
        }
    }
}
