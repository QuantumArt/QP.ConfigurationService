using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using QP.ConfigurationService.Models;
using Refit;

namespace QP.ConfigurationService.Client
{
    public class QPConfigurationService : IQPConfigurationService
    {
        private readonly IQPConfigurationService _service;

        public QPConfigurationService(string serviceUrl, string jwtToken)
        {
            _service = RestService.For<IQPConfigurationService>(
                new HttpClient(new JwtHttpClientHandler(jwtToken))
                {
                    BaseAddress = new Uri(serviceUrl)
                });
        }

        public virtual Task<CustomerConfiguration> GetCustomer(string id)
        {
            return _service.GetCustomer(id);
        }

        public virtual Task<List<CustomerConfiguration>> GetCustomers()
        {
            return _service.GetCustomers();
        }

        public virtual Task<List<ApplicationVariable>> GetVariables()
        {
            return _service.GetVariables();
        }

        private class JwtHttpClientHandler : HttpClientHandler
        {
            private readonly string _jwtToken;

            public JwtHttpClientHandler(string jwtToken)
            {
                _jwtToken = jwtToken;
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
