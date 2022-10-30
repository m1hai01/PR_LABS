using Waiter.Models;


namespace Waiter.Services
{
    public class WaiterService : IWaiter
    {
        private Queue<Order> queue = new Queue<Order>();
        private Queue<ReturnOrder> queue1 = new Queue<ReturnOrder>();
        private Mutex mutex = new Mutex();
        private Mutex mutex1 = new Mutex();
        Random rnd = new Random();
        private HttpClient httpClient;
        private readonly ILogger<WaiterService> _logger;
        private SemaphoreSlim _semaphoreReceiveOrder = new SemaphoreSlim(5,5);
        private SemaphoreSlim _semaphoreReceiveReturnOrder = new SemaphoreSlim(5,5);

        
        public WaiterService(ILogger<WaiterService> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://host.docker.internal:8080/");//go back to dining hall
                                                                                  // _logger.LogInformation($"Constructor start ");
            for (int i = 0; i < 5; i++)
            {
                //_logger.LogInformation($"Constructor-for  ");
                Task.Run(PrepareOrder);
                Task.Run(PrepareReturnOrder);
                Thread.Sleep(500);
            }

        }

        public async Task ReceiveOrder(Order order)
        {
            await _semaphoreReceiveOrder.WaitAsync();
            mutex.WaitOne();

            queue.Enqueue(order);
            _logger.LogInformation($"Queue count after order enqueue {queue.Count}.");
            
            mutex.ReleaseMutex();
            _semaphoreReceiveOrder.Release();
        }

        public async Task PrepareOrder()
        {
            while (true)
            {
                await _semaphoreReceiveOrder.WaitAsync();
                // lock the logic bellow for other threads not to enter (only one thread at a time)
                mutex.WaitOne();
                if (queue.Count != 0)
                {
                    _logger.LogInformation($"Entered prepare order.");
                    Thread.Sleep(5000);
                    var order = queue.Dequeue();
                    _logger.LogInformation($"Queue count after order dequeue {queue.Count}.");

                    SendOrder(order);
                }
                // allow other waiting threads to execute above logic
                mutex.ReleaseMutex();
                _semaphoreReceiveOrder.Release();
            }
        }

        public void SendOrder(Order order)
        {
            _logger.LogInformation($"Sending order {order.order_id}...");
            httpClient.PostAsJsonAsync("http://localhost:5172/Kitchen/Order", order);
            _logger.LogInformation($"Order {order.order_id} was sent to kitchen.");
        }

        public async Task ReceiveReturnOrder(ReturnOrder order)
        {
            await _semaphoreReceiveReturnOrder.WaitAsync();
            mutex1.WaitOne();
            _logger.LogInformation($"Receive Order{order.order_id} ");
            queue1.Enqueue(order);
            mutex1.ReleaseMutex();
            _semaphoreReceiveReturnOrder.Release();
        }

        public async Task PrepareReturnOrder()
        {
            //_logger.LogInformation($"PrepareOrder ");
            while (true)
            {
                await _semaphoreReceiveReturnOrder.WaitAsync();
                // _logger.LogInformation($"Prepare order while ");
                mutex1.WaitOne();//lock the logic bellow for other threads enter 
                if (queue1.Count != 0)
                {
                    //_logger.LogInformation($"prepare order if ");
                    _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(5000);
                    var order = queue1.Dequeue();
                    _logger.LogInformation($"Dequed order{order.order_id} ");
                   
                    SendReturnOrder(order);
                }
                mutex1.ReleaseMutex();//allow to other threads to enter in logic above 
                _semaphoreReceiveReturnOrder.Release();
            }
        }

        public void SendReturnOrder(ReturnOrder order)
        {
            _logger.LogInformation($"send  order{order.order_id} ");
            httpClient.PostAsJsonAsync("http://localhost:5257/DiningHall/Distribution", order);
        }

    }
}
