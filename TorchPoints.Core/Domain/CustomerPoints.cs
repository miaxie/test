using Dapper;
using System;
using TorchPoints.Core.Domain;

namespace TorchPoints.Core
{
    [Table("CustomerPoints")]
    public class CustomerPoints:BaseEntity
    {
        public int CustomerId { get; set; }
        public int Amount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
 