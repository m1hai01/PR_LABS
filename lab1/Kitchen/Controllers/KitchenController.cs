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
        public void Order([FromBody] Order order)
        {
            //_logger.LogWarning("warning");
            Task.Run(() => iKitchen.ReceiveOrder(order));

        }
        
    }
}