using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using QP.ConfigurationService.Client.Utils;
using QP.ConfigurationService.Models;

namespace QP.ConfigurationService.Client
{
    /// <summary>
    /// Thread-safe client for QP.ConfigurationService with caching.
    /// </summary>
    public class CachedQPConfigurationService : QPConfigurationService
    {
        private readonly TimeSpan _period;

        public CachedQPConfigurationService(string serviceUrl, string jwtToken)
            : this(serviceUrl, jwtToken, TimeSpan.FromMinutes(5))
        {
        }

        public CachedQPConfigurationService(string serviceUrl, string jwtToken, TimeSpan cachePeriod)
            : base(serviceUrl, jwtToken)
        {
            _period = cachePeriod;
        }
        
        public override Task<CustomerConfiguration> GetCustomer(string id)
        {
            return MemoryCache.Default.GetOrAddAsync(
                $"QPConfigurationService/customers/{id}",
                DateTimeOffset.Now.Add(_period),
                () => base.GetCustomer(id));
        }

        public override Task<List<CustomerConfiguration>> GetCustomers()
        {
            return MemoryCache.Default.GetOrAddAsync(
                $"QPConfigurationService/customers",
                DateTimeOffset.Now.Add(_period),
                () => base.GetCustomers());
        }

        public override Task<List<ApplicationVariable>> GetVariables()
        {
            return MemoryCache.Default.GetOrAddAsync(
                $"QPConfigurationService/variables",
                DateTimeOffset.Now.Add(_period),
                () => base.GetVariables());
        }
    }
}
