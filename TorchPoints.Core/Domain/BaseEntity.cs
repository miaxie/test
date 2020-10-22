using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.Domain
{
    [Serializable]
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }
    }
}
