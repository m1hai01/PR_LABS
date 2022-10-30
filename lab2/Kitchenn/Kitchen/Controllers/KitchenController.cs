using Kitchen.Interfaces;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KitchenController : ControllerBase
    {

        private readonly IKitchen iKitchen;
        private readonly ILogger<KitchenController> _logger;


        public KitchenController(ILogger<KitchenController> logger, IKitchen iKitchen)
        {
            _logger = logger;
            this.iKitchen = iKitchen;
        }
        
        [HttpPost("Order")]
        public async Task Order([FromBody] Order order)
        {
            _logger.LogInformation($"Kitchen controller Receive order");
            await iKitchen.ReceiveOrder(order);

        }
        
    }
}