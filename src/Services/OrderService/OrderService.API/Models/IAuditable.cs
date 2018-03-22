using System;

namespace OrderService.API.Models
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
