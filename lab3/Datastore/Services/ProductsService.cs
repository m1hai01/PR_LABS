using Datastore.Models;
using Datastore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Productstore.Services
{
    public class ProductsService : IProductsService
    {
        private List<Product> Product { get; set; } = new();

        public List<Product> GetProducts()
        {
            return Product;
        }

        public Product GetIdProduct(Guid id)
        {
            var productItem = Product.FirstOrDefault(x => x.Id == id);
            return productItem;
        }

        public Guid AddProduct(Product productItem)
        {
            var id = Guid.NewGuid();
            productItem.Id = id;
            Product.Add(productItem);

            return id;
        }

        public Product UpdateProduct(Guid id, Product productItem)
        {
            var localProduct = Product.FirstOrDefault(x => x.Id == id);

            localProduct.Name = productItem.Name;
            localProduct.Brand = productItem.Brand;

            return localProduct;
        }

        public void DeleteProduct(Guid id)
        {
            var productItem = Product.FirstOrDefault(x => x.Id == id);

            Product.Remove(productItem);
        }
    }
}
