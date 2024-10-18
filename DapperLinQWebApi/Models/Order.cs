namespace DapperLinQWebApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
