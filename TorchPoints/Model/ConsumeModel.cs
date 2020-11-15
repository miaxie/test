using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorchPoints.Core.Domain;

namespace TorchPoints.Model
{
    /// <summary>
    /// 积分历史输入model
    /// </summary>
    public class ConsumeModel
    {
        /// <summary>
        /// 积分数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public int ConsumeTypeId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
