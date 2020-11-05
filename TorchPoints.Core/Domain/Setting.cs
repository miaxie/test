using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.Domain
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeValue { get; set; }
    }
}
