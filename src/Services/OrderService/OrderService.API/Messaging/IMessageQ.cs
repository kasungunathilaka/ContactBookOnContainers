using OrderService.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.Messaging
{
    public interface IMessageQ
    {
        void SendMessage(CustomerViewModel customer);
    }
}
