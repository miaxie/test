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
    public class PointHistoryModel
    {
        /// <summary>
        /// 积分类型id
        /// </summary>
        public int TypeId { get; set; }
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
    }
}
