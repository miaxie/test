using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorchPoints.Core.Domain;

namespace website.Models
{
    /// <summary>
    /// 积分历史输入model
    /// </summary>
    public class PointHistoryListModel
    {
        public List<PointHistoryModel> Historys = new List<PointHistoryModel>();
    }
}
