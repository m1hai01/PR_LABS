using Datastore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Datastore.Services
{
    public interface IProductsService
    {
        //I will create an interface for my service which will be used to configure the service as
        //Dependency Injection in the form of Singleton class at the application level.
        public List<Product> GetProducts();

        //based on "guid" we identify an object in the list
        public Product GetIdProduct(Guid id);

        public Guid AddProduct(Product productItem);

        public Product UpdateProduct(Guid id, Product productItem);

        public void DeleteProduct(Guid id);

    }
}
