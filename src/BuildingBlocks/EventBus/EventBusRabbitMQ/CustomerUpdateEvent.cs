using Events.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events
{
    public class CustomerUpdateEvent 
    {
        public CustomerViewModel Customer { get; }

        public CustomerUpdateEvent(CustomerViewModel addedCustomer)
        {
            Customer = addedCustomer;
        }
    }
}
