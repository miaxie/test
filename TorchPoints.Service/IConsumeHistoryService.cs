using System;
using System.Collections.Generic;
using System.Text;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{
    public interface IConsumeHistoryService
    {
        /// <summary>
        /// 插入积分消费记录
        /// </summary>
        /// <param name="info"></param>
        string InsertConsumeHistory(ConsumeHistory info);

        /// <summary>
        /// 插入积分消费记录明细
        /// </summary>
        /// <param name="info"></param>
        void InsertConsumeDetail(ConsumeDetail info);
    }
}
