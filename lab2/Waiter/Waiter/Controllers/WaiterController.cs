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
            public async Task Distribution([FromBody] ReturnOrder order)
            {
                _logger.LogInformation($"Receive return order {order.order_id} from Kitchen ");
                
                await iWaiter.ReceiveReturnOrder(order);
            }

            [HttpPost("Order")]
            // receive incoming order from dinning hall
            public async Task Order([FromBody] Order order)
            {
                _logger.LogInformation($"Server received order {order.order_id} form dining hall.");
                await iWaiter.ReceiveOrder(order);
            }
        }
    }
}
