using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorchPoints.Core.Domain;

namespace website.Models
{
    /// <summary>
    /// 积分历史输入model
    /// </summary>
    public class PointHistoryModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 积分类型id
        /// </summary>
        public string TypeName { get; set; }
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
        /// 积分过期时间
        /// </summary>
        public DateTime ExpiredDate { get; set; }
        /// <summary>
        /// 积分状态id
        /// </summary>
        public string Status { get; set; }
    }
}
