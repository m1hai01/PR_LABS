using Waiter.Models;

namespace Waiter
{
    public interface IWaiter
    {
        Task ReceiveOrder(Order order);
        Task ReceiveReturnOrder(ReturnOrder order);
    }
}
