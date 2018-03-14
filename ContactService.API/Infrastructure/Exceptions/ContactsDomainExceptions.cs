using System;

namespace ContactService.API.Infrastructure.Exceptions
{
    public class ContactsDomainExceptions : Exception
    {
        public ContactsDomainExceptions()
        { }

        public ContactsDomainExceptions(string message)
            : base(message)
        { }

        public ContactsDomainExceptions(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
