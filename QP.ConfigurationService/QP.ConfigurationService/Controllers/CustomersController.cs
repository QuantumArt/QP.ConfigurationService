using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QP.ConfigurationService.Services;

namespace QP.ConfigurationService.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : Controller
    {
        ICustomersConfigurationsService _customersConfigurationsService;

        public CustomersController(ICustomersConfigurationsService customersConfigurationsService)
        {
            _customersConfigurationsService = customersConfigurationsService;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var result = _customersConfigurationsService.GetCustomerConfig(id);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }
    }
}
