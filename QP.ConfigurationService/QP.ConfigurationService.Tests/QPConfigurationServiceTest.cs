using Microsoft.VisualStudio.TestTools.UnitTesting;
using QP.ConfigurationService.Client;
using System.Threading.Tasks;

namespace QP.ConfigurationService.Tests
{
    [TestClass]
    public class QPConfigurationServiceTest
    {
        const string Url = "http://mscdev02:54321";
        const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJRUC5Db25maWd1cmF0aW9uU2VydmljZS5Jc3N1ZXIiLCJleHAiOjE1Nzg1ODM4NjIsImF1ZCI6IlFQLkNvbmZpZ3VyYXRpb25TZXJ2aWNlLkF1ZGllbmNlIn0.wN-we_Wezxtv3rtLn3mEQ9tZngPwfgn45Ta7kLfKO-Q";

        [TestMethod]
        public async Task GetCustomerTest()
        {
            var service = new QPConfigurationService(Url, Token);

            var customer = await service.GetCustomer("mts_catalog");

            Assert.IsNotNull(customer);
            Assert.AreEqual("mts_catalog", customer.Name);
        }

        [TestMethod]
        public async Task GetCustomersTest()
        {
            var service = new QPConfigurationService(Url, Token);

            var customers = await service.GetCustomers();

            Assert.IsNotNull(customers);
            Assert.IsNotNull(customers.Find(c => c.Name == "mts_catalog"));
            Assert.IsNotNull(customers.Find(c => c.Name == "qp_beeline_dpc"));
        }

        [TestMethod]
        public async Task GetVariablesTest()
        {
            var service = new QPConfigurationService(Url, Token);

            var variables = await service.GetVariables();

            Assert.IsNotNull(variables);
            Assert.IsNotNull(variables.Find(v => v.Name == "ApplicationTitle"));
            Assert.IsNotNull(variables.Find(v => v.Name == "TempDirectory"));
        }
    }
}
