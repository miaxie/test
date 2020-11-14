using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.Domain
{
    [Table("ConsumeDetail")]
    public class ConsumeDetail : BaseEntity
    {
        /// <summary>
        /// 积分历史Id => PointHistory表
        /// </summary>
        public int PointHistoryId { get; set; }
        /// <summary>
        /// 消费历史表Id
        /// </summary>
        public int ConsumeHistoryId { get; set; }
        /// <summary>
        /// 消费积分数（每次消费有可能用到多笔积分历史，分开记录）
        /// </summary>
        public int Amount { get; set; }
    }
}
