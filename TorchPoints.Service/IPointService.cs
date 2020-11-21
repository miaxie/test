using System;
using System.Collections.Generic;
using System.Data;
using TorchPoints.Core;
using TorchPoints.Core.DataAccess;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{
    public interface IPointService
    {
        void InsertCustomerPoint(CustomerPoints customerPoints, IDbTransaction transaction = null);
        void UpdateCustomerPoint(CustomerPoints customerPoints, IDbTransaction transaction = null);
        void DeleteCustomerPoint(CustomerPoints customerPoints);
        void DeleteCustomerPointsByIds(int[] ids);
        CustomerPoints GetCustomerPointsbyId(int id);
        CustomerPoints GetCustomerPointsbyCustomerId(int customerId);
        IEnumerable<CustomerPoints> GetAllCustomerPoints(int customerId = 0);

        #region 会员积分历史操作
        PointHistory InsertPointHistory(PointHistory history);
        void UpdatePointHistory(PointHistory history);
        void DeletePointHistory(PointHistory history);
        PointHistory GetPointHistorybyId(int id);
        IPagedList<PointHistory> GetAllPointHistory(int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取有效积分记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<PointHistory> GetUnUsedPointHistory(int customerId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
        #endregion
    }
}
