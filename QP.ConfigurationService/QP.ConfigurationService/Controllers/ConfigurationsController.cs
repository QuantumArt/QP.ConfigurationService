using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QP.ConfigurationService.Services;

namespace QP.ConfigurationService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ConfigurationsController : Controller
    {
        ICustomersConfigurationsService _customersConfigurationsService;

        public ConfigurationsController(ICustomersConfigurationsService customersConfigurationsService)
        {
            _customersConfigurationsService = customersConfigurationsService;
        }

        [Authorize]
        [HttpGet("customers/{id}")]
        public ActionResult GetCustomerConfiguration(string id)
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
        public ActionResult GetCustomers()
        {
            return Json(_customersConfigurationsService.GetCustomersNames());
        }
    }
}
