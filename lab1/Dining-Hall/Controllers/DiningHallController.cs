using Dining_Hall.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dining_Hall.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiningHallController : ControllerBase
    {
     

        private readonly ILogger<DiningHallController> _logger;

        public DiningHallController(ILogger<DiningHallController> logger)
        {
            _logger = logger;
        }

        //freastra la care putem sa ne adresam
        [HttpPost("Distribution")]
        public void Distribution([FromBody] ReturnOrder order)
        {
            _logger.LogInformation($"Received order {order.order_id}");
        }

    }
}