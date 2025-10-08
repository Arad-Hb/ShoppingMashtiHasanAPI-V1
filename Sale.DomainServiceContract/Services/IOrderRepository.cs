using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.DomainServiceContract.Services
{
    public interface IOrderRepository
    {
        bool HasOrder(int ProductID);
    }
}
