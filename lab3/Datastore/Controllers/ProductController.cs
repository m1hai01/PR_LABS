using Datastore.Models;
using Datastore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Datastore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private IProductsService _service;


        public ProductsController( IProductsService service)
        {
            _service = service;

        }

        [HttpGet("products")]
        public List<Product> GetProducts()
        {
            return _service.GetProducts();
        }

        [HttpGet("products/{id}")]
        public Product GetIdProduct(Guid id)
        {
            return _service.GetIdProduct(id);
        }


        [HttpPost("products")]
        public Guid AddProduct(Product product)
        {
            return _service.AddProduct(product); 
        }


        [HttpPut("products/{id}")]
        public Product UpdateProduct(Guid id, Product product)
        {
            return _service.UpdateProduct(id, product);
        }


        [HttpDelete("products/{id}")]
        public void DeleteProduct(Guid id)
        {
            
             _service.DeleteProduct(id);
        }
    }
}
