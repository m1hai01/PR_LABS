namespace Waiter.Models
{
    public class Order
    {
        public int order_id { get; set; }
        public int table_id { get; set; }
        public int waiter_id { get; set; }
        public int[] items { get; set; }
        public int priority { get; set; }
        public int max_wait { get; set; }
        public DateTime pick_up_timev { get; set; } = DateTime.Now;
    }
}
