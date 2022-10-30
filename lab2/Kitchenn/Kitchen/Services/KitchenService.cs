using Kitchen.Interfaces;
using Kitchen.Models;

namespace Kitchen.Services
{
    public class KitchenService : IKitchen
    {
        private Queue<Order> queue = new Queue<Order>();
        private Mutex mutex = new Mutex();
        Random rnd = new Random();
        private HttpClient httpClient;
        private readonly ILogger<KitchenService> _logger;
        private SemaphoreSlim _semaphoreKitchen = new SemaphoreSlim(5, 5);
        public KitchenService(ILogger<KitchenService> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5166");//go back to dining hall
           // _logger.LogInformation($"Constructor start ");
            for (int i = 0; i < 5; i++)
            {
                //_logger.LogInformation($"Constructor-for  ");
                Task.Run(PrepareOrder);
            }
        
        }

        public async Task ReceiveOrder(Order order)
        {
            await _semaphoreKitchen.WaitAsync();
            mutex.WaitOne();
            _logger.LogInformation($"Receive Order{order.order_id} ");
            queue.Enqueue(order);
            mutex.ReleaseMutex();
            _semaphoreKitchen.Release();
        }

        public async Task PrepareOrder()
        {
            //_logger.LogInformation($"PrepareOrder ");
            while (true)
            {
                await _semaphoreKitchen.WaitAsync();
               // _logger.LogInformation($"Prepare order while ");
                mutex.WaitOne();//lock the logic bellow for other threads enter 
                if (queue.Count != 0)
                {
                    //_logger.LogInformation($"prepare order if ");
                    _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId}");
                    var order = queue.Dequeue();
                    _logger.LogInformation($"Dequed order{order.order_id} ");
                    var cookingtime = Convert.ToInt32(order.max_wait * 1.3);
                    var items = order.items;
                    

                    var returnOrder = new ReturnOrder
                    {
                        order_id = order.order_id,
                        waiter_id = order.waiter_id,
                        priority = order.priority,
                        items = order.items,
                        table_id = order.table_id,
                        max_wait = order.max_wait,
                        cooking_time = cookingtime,
                    };
                    SendReturnOrder(returnOrder);
                }
                mutex.ReleaseMutex();//allow to other threads to enter in logic above 
                _semaphoreKitchen.Release();
            }
        }

        public void SendReturnOrder(ReturnOrder returnOrder)
        {
            _logger.LogInformation($"send return order{returnOrder.order_id} ");
            httpClient.PostAsJsonAsync("Waiter/Distribution", returnOrder);
        }
    }
}
