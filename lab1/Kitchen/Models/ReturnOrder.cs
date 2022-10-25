namespace Kitchen.Models
{
    public class ReturnOrder
    {
        public int order_id { get; set; }
        public int table_id { get; set; }
        public int waiter_id { get; set; }
        public int[] items { get; set; }
        public int priority { get; set; }
        public int max_wait { get; set; }
        public DateTime pick_up_timev { get; set; } = DateTime.Now;

        public  int cooking_time { get; set; }
        public List<CookingDetails> cooking_details { get; set; }


    }
}
