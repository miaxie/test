using System;
using System.Collections.Generic;
using System.Text;
using TorchPoints.Core.DataAccess;
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

        /// <summary>
        /// 获取积分消费历史
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ConsumeHistory> GetAllConsumeHistorys(int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        }
}
