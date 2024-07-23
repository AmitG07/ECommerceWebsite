using Microsoft.AspNetCore.Mvc;
using Order.Model;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private static List<OrderModel> orders = new List<OrderModel>
        {
            new OrderModel
            {
                OrderId = "O001",
                CustomerId = 1,
                OrderedOn = DateTime.Parse("2024-02-10"),
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 1999,
            },
            new OrderModel
            {
                OrderId = "O002",
                CustomerId = 1,
                OrderedOn = DateTime.Parse("2024-02-11"),
                ProductId = 2,
                Quantity = 2,
                UnitPrice = 30000,
            }
        };

        // GET: api/order
        [HttpGet]
        public ActionResult<IEnumerable<OrderModel>> GetOrders()
        {
            return Ok(orders);
        }

        // POST: api/order
        [HttpPost]
        public ActionResult<OrderModel> CreateOrder(OrderModel order)
        {
            // Generate a new OrderId
            order.OrderId = Guid.NewGuid().ToString();
            orders.Add(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(string id, OrderModel order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            var existingOrder = orders.FirstOrDefault(o => o.OrderId == id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderedOn = order.OrderedOn;
            existingOrder.ProductId = order.ProductId;
            existingOrder.Quantity = order.Quantity;
            existingOrder.UnitPrice = order.UnitPrice;

            return NoContent();
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            orders.Remove(order);
            return NoContent();
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrderById(string id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}