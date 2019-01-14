using Microsoft.VisualStudio.TestTools.UnitTesting;
using QP.ConfigurationService.Client;
using System;
using System.Threading.Tasks;

namespace QP.ConfigurationService.Tests
{
    [TestClass]
    public class CachedQPConfigurationServiceTest
    {
        const string Url = "http://mscdev02:54321";
        const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJRUC5Db25maWd1cmF0aW9uU2VydmljZS5Jc3N1ZXIiLCJleHAiOjE1Nzg1ODM4NjIsImF1ZCI6IlFQLkNvbmZpZ3VyYXRpb25TZXJ2aWNlLkF1ZGllbmNlIn0.wN-we_Wezxtv3rtLn3mEQ9tZngPwfgn45Ta7kLfKO-Q";

        [TestMethod]
        public async Task GetCustomerTest()
        {
            var service = new CachedQPConfigurationService(Url, Token, TimeSpan.FromMilliseconds(100));

            var customerTask = service.GetCustomer("mts_catalog");

            await Task.Delay(10);

            var customer2Task = service.GetCustomer("mts_catalog");

            await customerTask;
            await customer2Task;

            var customer = customerTask.Result;
            var customer2 = customer2Task.Result;

            Assert.IsNotNull(customer);
            Assert.AreEqual("mts_catalog", customer.Name);
            Assert.AreSame(customer, customer2);

            await Task.Delay(200);

            var customer3 = await service.GetCustomer("mts_catalog");

            Assert.IsNotNull(customer3);
            Assert.AreEqual("mts_catalog", customer3.Name);
            Assert.AreNotSame(customer, customer3);
        }

        [TestMethod]
        public async Task GetCustomersTest()
        {
            var service = new CachedQPConfigurationService(Url, Token, TimeSpan.FromMilliseconds(100));

            var customersTask = service.GetCustomers();

            await Task.Delay(10);

            var customers2Task = service.GetCustomers();

            await customersTask;
            await customers2Task;

            var customers = customersTask.Result;
            var customers2 = customers2Task.Result;

            Assert.IsNotNull(customers);
            Assert.IsNotNull(customers.Find(c => c.Name == "mts_catalog"));
            Assert.IsNotNull(customers.Find(c => c.Name == "qp_beeline_dpc"));
            Assert.AreSame(customers, customers2);

            await Task.Delay(200);

            var customers3 = await service.GetCustomers();

            Assert.IsNotNull(customers3);
            Assert.IsNotNull(customers3.Find(c => c.Name == "mts_catalog"));
            Assert.IsNotNull(customers3.Find(c => c.Name == "qp_beeline_dpc"));
            Assert.AreNotSame(customers, customers3);
        }
                
        [TestMethod]
        public async Task GetVariablesTest()
        {
            var service = new CachedQPConfigurationService(Url, Token, TimeSpan.FromMilliseconds(100));

            var variablesTask = service.GetVariables();

            await Task.Delay(10);

            var variables2Task = service.GetVariables();

            await variablesTask;
            await variables2Task;

            var variables = variablesTask.Result;
            var variables2 = variables2Task.Result;

            await Task.Delay(200);

            var variables3 = await service.GetVariables();

            Assert.IsNotNull(variables);
            Assert.IsNotNull(variables.Find(v => v.Name == "ApplicationTitle"));
            Assert.IsNotNull(variables.Find(v => v.Name == "TempDirectory"));
            Assert.AreSame(variables, variables2);

            Assert.IsNotNull(variables3);
            Assert.IsNotNull(variables3.Find(v => v.Name == "ApplicationTitle"));
            Assert.IsNotNull(variables3.Find(v => v.Name == "TempDirectory"));
            Assert.AreNotSame(variables, variables3);
        }
    }
}
