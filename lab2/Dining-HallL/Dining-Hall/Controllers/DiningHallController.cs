using Dining_Hall.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dining_Hall.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiningHallController : ControllerBase
    {
        private readonly ILogger<DiningHallController> _logger;
        private readonly ISemaphore iSemaphore;

        public DiningHallController(ILogger<DiningHallController> logger, ISemaphore iSemaphore)
        {
            _logger = logger;
            this.iSemaphore = iSemaphore;
        }

        //freastra la care putem sa ne adresam
        [HttpPost("Distribution")]
        public void Distribution([FromBody] ReturnOrder order)
        {
            _logger.LogInformation($"Received order {order.order_id}");
        }


        //test
        [HttpPost("Stop")]
        public void Stop()
        {
            _logger.LogInformation($"Thread stop POST");
            Task.Run(() => iSemaphore.Stop());
        }


        [HttpPost("Start")]
        public void Start()
        {
            _logger.LogInformation($"Thread start POST ");
            Task.Run(() => iSemaphore.Start());
        }
    }
}