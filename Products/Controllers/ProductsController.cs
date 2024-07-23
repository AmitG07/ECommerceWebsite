using Microsoft.AspNetCore.Mvc;
using Products.Model;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Products.Rabbitmq;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;

        public ProductsController(IRabbitMqService rabbitMqService)
        {   
            _rabbitMqService = rabbitMqService;
        }

        private static List<ProductModel> productModels = new List<ProductModel>
        {
            new ProductModel
            {
                ProductId = 1,
                ProductName = "Noise Icon Buzz",
                CategoryId = "C001",
                CategoryName = "Smartwatch",
                Design = "Metal Finish",
                Size = "45mm",
                Price = 199,
                Description = "Smartwatch by Noise",
                Image = "https://rukminim2.flixcart.com/image/832/832/xif0q/smartwatch/y/9/l/-original-imagxp8u2fgthyxy.jpeg?q=70&crop=false"
            },
            new ProductModel
            {
                ProductId = 2,
                ProductName = "OnePlus Nord 3 5G",
                CategoryId = "C002",
                CategoryName = "Smartphone",
                Design = "Glass Body",
                Size = "6.74 inch",
                Price = 30000,
                Description = "Tempest Gray",
                Image = "https://rukminim2.flixcart.com/image/832/832/xif0q/mobile/a/y/i/nord-3-5g-cph2491-oneplus-original-imagrk2vbpbegxmy.jpeg?q=70&crop=false"
            },
        };

        [HttpGet]
        public ActionResult<IEnumerable<ProductModel>> GetProducts()
        {
            return Ok(productModels);
        }

        [HttpGet("{productId:int}")]
        public IActionResult GetById(int productId)
        {
            var product = productModels.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> Create(ProductModel product)
        {
            // Generate a new ProductId
            int newProductId = productModels.Max(p => p.ProductId) + 1;
            product.ProductId = newProductId;
            productModels.Add(product);
            return CreatedAtAction(nameof(GetById), new { productId = newProductId }, product);
        }

        [HttpPut("{productId:int}")]
        public IActionResult Update(int productId, ProductModel product)
        {
            var existingProduct = productModels.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.CategoryName = product.CategoryName;
            existingProduct.Price = product.Price;
            existingProduct.Design = product.Design;
            existingProduct.Size = product.Size;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;

            return NoContent();
        }

        [HttpDelete("{productId:int}")]
        public IActionResult Delete(int productId)
        {
            var product = productModels.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            productModels.Remove(product);
            return NoContent();
        }

        [HttpPost("AddToOrder")]
        public IActionResult AddToOrder([FromBody] AddToOrder request)
        {
            if (request.ProductId < 0)
            {
                return BadRequest();
            }
            var product = productModels.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            _rabbitMqService.SendingMessage<ProductModel>(product);
            //_messageProducer.SendingMessage<PublishProduct>(newPublishProduct);

            return Ok(productModels);
        }
    }
}
