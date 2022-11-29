using System.Text.Json.Serialization;

namespace Datastore.Models
{
    public class Product
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
    }
}
