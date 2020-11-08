using System;
using System.Collections.Generic;
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
        NoUsed = 0,
        /// <summary>
        /// 已使用
        /// </summary>
        Used = 2,
        /// <summary>
        /// 过期
        /// </summary>
        Expired = 3

    }
}
