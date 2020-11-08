using System;
using System.Collections.Generic;
using System.Text;
using TorchPoints.Core.Domain.Enum;

namespace TorchPoints.Core.Domain
{
    /// <summary>
    /// 积分历史表
    /// </summary>
    public class PointHistory : BaseEntity
    {
        /// <summary>
        /// 积分类型id
        /// </summary>
        public PointSourceType TypeId { get; set; }
        /// <summary>
        /// 积分数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 获赠时间
        /// </summary>
        public DateTime GetTime { get; set; }
        /// <summary>
        /// 积分使用时间
        /// </summary>
        public DateTime? UsedDate { get; set; }
        /// <summary>
        /// 积分过期时间
        /// </summary>
        public DateTime ExpiredDate { get; set; }
        /// <summary>
        /// 积分状态id
        /// </summary>
        public PointStatus StatusId { get; set; }
    }
}
