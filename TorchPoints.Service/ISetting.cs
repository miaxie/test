using System;
using System.Collections.Generic;
using System.Text;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{
    public interface  ISetting
    {
        Setting GetSettingByName(string name);
    }
}
