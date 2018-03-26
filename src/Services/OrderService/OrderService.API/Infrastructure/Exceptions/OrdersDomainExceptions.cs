using System;

namespace OrderService.API.Infrastructure.Exceptions
{
    public class OrdersDomainExceptions : Exception
    {
        public OrdersDomainExceptions()
        { }

        public OrdersDomainExceptions(string message)
            : base(message)
        { }

        public OrdersDomainExceptions(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
