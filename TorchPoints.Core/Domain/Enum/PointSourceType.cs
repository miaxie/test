using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TorchPoints.Core.Domain.Enum
{
   public enum PointSourceType
    {
        /// <summary>
        /// 赠送
        /// </summary>
        [Description("赠送")]
        Gift =1,
        /// <summary>
        /// 系统赠送
        /// </summary>
        [Description("系统赠送")] 
        System =2

    }
}
