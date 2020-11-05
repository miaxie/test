using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.Domain
{
    public class ConsumeHistory : BaseEntity
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 消费积分总额
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 消费日期
        /// </summary>
        public DateTime ConsumDate { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int ConsumeTypeId { get; set; }
    }
}
