using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.API.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private IOrdersService _service;
        private ILogger<OrdersController> _logger;
        public OrdersController(IOrdersService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
