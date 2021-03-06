﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.Domain
{
    [Table("ExpiresPoint")]
    public class ExpiresPoint : BaseEntity
    {
        /// <summary>
        /// 积分历史表id
        /// </summary>
        public int PointHistoryId { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 过期积分数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 迁移时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 剩余积分数量
        /// </summary>
        public int RemainAmount { get; set; }
    }
}
