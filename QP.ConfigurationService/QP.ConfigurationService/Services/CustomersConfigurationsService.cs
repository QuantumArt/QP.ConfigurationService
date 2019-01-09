using Microsoft.Extensions.Configuration;
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
    }

    public class CustomersConfigurationsService : ICustomersConfigurationsService
    {
        const string ConfigFilePathKey = "QpConfigurationPath";

        string _configFilePath;
        FileSystemWatcher _configFileWatcher;
        XmlSerializer _configSerializer = new XmlSerializer(typeof(Configuration));
        Dictionary<string, CustomerConfiguration> _customersConfigurations = new Dictionary<string, CustomerConfiguration>();

        public CustomersConfigurationsService(IConfiguration configuration)
        {

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

        void UpdateConfigurations()
        {
            try
            {
                using (var xmlTextReader = new XmlTextReader(_configFilePath))
                {
                    var config = (Configuration)_configSerializer.Deserialize(xmlTextReader);
                    _customersConfigurations = config.Customers.ToDictionary(c => c.Name, c => c);
                }
            }
            catch (Exception ex)
            {
                // TODO: log
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
            }
            catch (Exception ex)
            {
                // TODO: log
            }
        }
    }
}
