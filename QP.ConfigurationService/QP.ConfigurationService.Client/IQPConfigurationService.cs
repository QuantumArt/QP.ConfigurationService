using System.Collections.Generic;
using System.Threading.Tasks;
using QP.ConfigurationService.Models;
using Refit;

namespace QP.ConfigurationService.Client
{
    interface IQPConfigurationService
    {
        [Get("/api/v1/customers/{id}")]
        Task<CustomerConfiguration> GetCustomer(string id);

        [Get("/api/v1/customers")]
        Task<List<CustomerConfiguration>> GetCustomers();

        [Get("/api/v1/variables")]
        Task<List<ApplicationVariable>> GetVariables();
    }
}
