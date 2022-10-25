using Waiter.Models;

namespace Waiter
{
    public interface IWaiter
    {
        void ReceiveOrder(Order order);
        void ReceiveReturnOrder(ReturnOrder order);
    }
}
