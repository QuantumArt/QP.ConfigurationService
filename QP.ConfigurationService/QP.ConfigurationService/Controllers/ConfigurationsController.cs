using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QP.ConfigurationService.Models;
using QP.ConfigurationService.Services;

namespace QP.ConfigurationService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ConfigurationsController : Controller
    {
        IQpConfigurationService _customersConfigurationsService;

        public ConfigurationsController(IQpConfigurationService customersConfigurationsService)
        {
            _customersConfigurationsService = customersConfigurationsService;
        }

        [Authorize]
        [HttpGet("customers/{id}")]
        public ActionResult<CustomerConfiguration> GetCustomerConfiguration(string id)
        {
            var result = _customersConfigurationsService.GetCustomerConfig(id);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [Authorize]
        [HttpGet("customers")]
        public ActionResult<ICollection<CustomerConfiguration>> GetCustomers()
        {
            return Json(_customersConfigurationsService.GetCustomersConfigs());
        }

        [Authorize]
        [HttpGet("variables")]
        public ActionResult<ICollection<ApplicationVariable>> GetVariables()
        {
            return Json(_customersConfigurationsService.GetVariables());
        }
    }
}
