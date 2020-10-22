using System;
using TorchPoints.Core.Domain;

namespace TorchPoints.Core
{
    public class CustomerPoints:BaseEntity
    {
        public int CustomerId { get; set; }
        public int Amount { get; set; }
    }
}
