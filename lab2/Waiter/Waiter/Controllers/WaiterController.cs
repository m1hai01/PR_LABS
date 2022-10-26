using Microsoft.AspNetCore.Mvc;
using Waiter.Models;

namespace Waiter.Controllers
{
    namespace Dining_Hall.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class WaiterController : ControllerBase
        {


            private readonly ILogger<WaiterController> _logger;
            private readonly IWaiter iWaiter;

            public WaiterController(ILogger<WaiterController> logger, IWaiter iWaiter)
            {
                _logger = logger;
                this.iWaiter = iWaiter;
            }

            //freastra la care putem sa ne adresam
            [HttpPost("Distribution")]
            public void Distribution([FromBody] ReturnOrder order)
            {
                Task.Run(() => iWaiter.ReceiveReturnOrder(order));
            }

            [HttpPost("Order")]
            // receive incoming order from dinning hall
            public void Order([FromBody] Order order)
            {
                _logger.LogInformation($"Server received order {order.order_id} form dining hall.");
                Task.Run(() => iWaiter.ReceiveOrder(order));
            }
        }
    }
}
