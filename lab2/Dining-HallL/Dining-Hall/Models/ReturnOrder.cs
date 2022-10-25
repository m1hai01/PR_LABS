namespace Dining_Hall.Models
{
    public class ReturnOrder
    {
        // ca sa stim ce model de clasa vine in controller
        public int order_id { get; set; }
        public int table_id { get; set; }
        public int waiter_id { get; set; }
        public int[] items { get; set; }
        public int priority { get; set; }
        public int max_wait { get; set; }
        public DateTime pick_up_timev { get; set; } = DateTime.Now;

        public int cooking_time  { get; set; }


    }
}
