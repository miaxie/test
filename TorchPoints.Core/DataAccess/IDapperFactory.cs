using System;
using System.Collections.Generic;
using System.Text;

namespace TorchPoints.Core.DataAccess
{
   public interface IDapperFactory
    {
        DapperClient CreateClient(string name);
    }
}
