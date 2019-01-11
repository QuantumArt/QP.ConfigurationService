﻿using Microsoft.Extensions.Configuration;
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
    public interface IQpConfigurationService
    {
        CustomerConfiguration GetCustomerConfig(string customerName);
        ICollection<CustomerConfiguration> GetCustomersNames();
        ICollection<ApplicationVariable> GetVariables();
    }

    public class QpConfigurationService : IQpConfigurationService
    {
        const string ConfigFilePathKey = "QpConfigurationPath";

        readonly ILogger<QpConfigurationService> _logger;

        string _configFilePath;
        FileSystemWatcher _configFileWatcher;
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

            SubscribeOnConfigChanges();
            UpdateConfigurations();
        }

        public CustomerConfiguration GetCustomerConfig(string customerName)
        {
            return _customersConfigurations.GetValueOrDefault(customerName);
        }

        public ICollection<CustomerConfiguration> GetCustomersNames()
        {
            return _customersConfigurations.Values;
        }

        public ICollection<ApplicationVariable> GetVariables()
        {
            return _variables.Values;
        }

        void UpdateConfigurations()
        {
            try
            {
                using (var xmlTextReader = new XmlTextReader(_configFilePath))
                {
                    var config = (Configuration)_configSerializer.Deserialize(xmlTextReader);
                    _customersConfigurations = config.Customers.ToDictionary(c => c.Name, c => c);
                    _variables = config.Variables.ToDictionary(v => v.Name, v => v);
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
