using System.ComponentModel.DataAnnotations;

namespace Order.Model
{
    public class OrderModel
    {
        [Key]
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderedOn { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
