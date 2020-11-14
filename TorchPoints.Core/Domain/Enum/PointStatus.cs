using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TorchPoints.Core.Domain.Enum
{
    /// <summary>
    /// 积分状态
    /// </summary>
   public enum PointStatus
    {
        /// <summary>
        /// 未使用
        /// </summary>
        [Description("未使用")]
        NoUsed = 0,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used = 2,
        /// <summary>
        /// 部分已使用
        /// </summary>
        [Description("部分已使用")]
        PartialUsed = 3,
        /// <summary>
        /// 过期
        /// </summary>
        [Description("过期")]
        Expired = 4

    }
}
