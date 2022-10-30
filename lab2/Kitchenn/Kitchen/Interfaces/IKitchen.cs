using Kitchen.Models;

namespace Kitchen.Interfaces
{
    public interface IKitchen
    {
        Task ReceiveOrder(Order order);
    }
}
