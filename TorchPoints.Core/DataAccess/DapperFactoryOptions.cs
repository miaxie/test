using System;
using System.Collections.Generic;

namespace TorchPoints.Core.DataAccess
{
    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }
}