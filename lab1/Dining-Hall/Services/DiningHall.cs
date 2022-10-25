using Dining_Hall.Models;
using RestSharp;

namespace Dining_Hall.Services
{
    public class DiningHall : IHall
    {
        static int waiter_id = 1;
        static int order_id = 1;
        private int[] tables = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private int[] items = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private int[] priority = new[] {1, 2, 3, 4, 5};
        private Random rnd;
        private HttpClient httpClient;
        private readonly ILogger<DiningHall> _logger;
        

        public DiningHall(ILogger<DiningHall> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            //where request go
            httpClient.BaseAddress = new Uri("http://host.docker.internal:8081/");
            //_logger.LogInformation($"Constructor start ");

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                // _logger.LogInformation($"Enter for loop for constructor ");

                // turn on threads that generate orders
                Task.Run(() => GenerateOrder());
            }

            
        }
        public void GenerateOrder()
        {
            //in body of request we generate order and it contain:
            rnd = new Random();
            var priority = this.priority[rnd.Next(0, 5)];
            var tables = this.tables[rnd.Next(0, 10)];
            var nr_items = rnd.Next(1, 6);
            int[] order_items = new int[nr_items];
            //_logger.LogInformation($"Generate order ");

            for (int i = 0; i < nr_items; i++)
            {
                //_logger.LogInformation($"For in generate order");
                order_items[i] = items[rnd.Next(0, 10)];
            }
            while (true)
            {
                //use while for permanently sending orders(for program doesn't stop)
                Thread.Sleep(5000);// ca sa sbiveasca abaroatele
                //_logger.LogInformation($"While in generate order ");
                var order = new Order
                {
                    waiter_id = waiter_id,
                    order_id = order_id++,
                    priority = priority,
                    items = order_items,
                    table_id = tables,
                    max_wait = rnd.Next(1, 50)

                };
                
                Task.Run(() => SendOrder(order));
                Thread.Sleep(2000);
            }
        }

        public void SendOrder(Order order)
        {
            _logger.LogInformation($"SendOrder{order.order_id} ");
            httpClient.PostAsJsonAsync("Kitchen/Order", order);//in body of request add order as json 

        }

    }
}
