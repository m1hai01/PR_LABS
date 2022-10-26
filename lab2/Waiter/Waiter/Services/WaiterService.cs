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

        private bool isStoped = false;
        private int min = 5;
        private int max = 20;
        
        public WaiterService(ILogger<WaiterService> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://host.docker.internal:8080/");//go back to dining hall
                                                                                  // _logger.LogInformation($"Constructor start ");
            for (int i = 0; i < 5; i++)
            {
                //_logger.LogInformation($"Constructor-for  ");
                Task.Run(() => PrepareOrder());
               // Task.Run(() => PrepareReturnOrder());
            }

        }

        public void ReceiveOrder(Order order)
        {
            //mutex.WaitOne();

            queue.Enqueue(order);
            _logger.LogInformation($"Queue count after order enqueue {queue.Count}.");

            if (queue.Count >= max && !isStoped)
            {
                _logger.LogInformation($"Entered if for sending stop request.");
                SendStop();
                isStoped = true;
            }

            //mutex.ReleaseMutex();
        }

        public void PrepareOrder()
        {
            while (true)
            {
                // lock the logic bellow for other threads not to enter (only one thread at a time)
                mutex.WaitOne();
                if (queue.Count != 0)
                {
                    _logger.LogInformation($"Entered prepare order.");

                    var order = queue.Dequeue();
                    _logger.LogInformation($"Queue count after order dequeue {queue.Count}.");

                    if (queue.Count < min && isStoped)
                    {
                        _logger.LogInformation($"Entered if for sending start request.");
                        SentStart();
                        isStoped = false;
                    }

                    Task.Run(() => SendOrder(order));
                    Thread.Sleep(8000);
                }
                // allow other waiting threads to execute above logic
                mutex.ReleaseMutex();
            }
        }

        public void SendOrder(Order order)
        {
            _logger.LogInformation($"Sending order {order.order_id}...");
            //httpClient.PostAsJsonAsync("http://localhost:5172/Kitchen/Order", order);
            _logger.LogInformation($"Order {order.order_id} was sent to kitchen.");
        }

        public void ReceiveReturnOrder(ReturnOrder order)
        {
            mutex1.WaitOne();
            _logger.LogInformation($"Receive Order{order.order_id} ");
            queue1.Enqueue(order);
            mutex1.ReleaseMutex();
        }

        public void PrepareReturnOrder()
        {
            //_logger.LogInformation($"PrepareOrder ");
            while (true)
            {
                // _logger.LogInformation($"Prepare order while ");
                mutex1.WaitOne();//lock the logic bellow for other threads enter 
                if (queue1.Count != 0)
                {
                    //_logger.LogInformation($"prepare order if ");
                    _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId}");
                    var order = queue1.Dequeue();
                    _logger.LogInformation($"Dequed order{order.order_id} ");
                    var cookingtime = Convert.ToInt32(order.max_wait * 1.3);
                    var items = order.items;


                   // Task.Run(() => SendReturnOrder(order));
                }
                mutex1.ReleaseMutex();//allow to other threads to enter in logic above 
            }
        }

        public void SendReturnOrder(ReturnOrder order)
        {
            _logger.LogInformation($"send  order{order.order_id} ");
            httpClient.PostAsJsonAsync("http://localhost:5257/DiningHall/Distribution", order);
        }

        public void SendStop()
        {
            _logger.LogInformation($"Sending stop request to dinning hall...");
            httpClient.PostAsync("http://localhost:5257/DiningHall/Stop", null);
            _logger.LogInformation($"Stop request was sent to dinning hall.");
        }

        public void SentStart()
        {
            _logger.LogInformation($"Sending start request to dinning hall...");
            httpClient.PostAsync("http://localhost:5257/DiningHall/Start", null);
            _logger.LogInformation($"Start request was sent to dinning hall.");
        }
    }
}
