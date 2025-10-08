using Sale.DomainServiceContract.Services;

namespace Infrastructue.Sale
{
    public class OrderRepository : IOrderRepository
    {
        public bool HasOrder(int ProductID)
        {
           
            return false;
        }
    }
}
