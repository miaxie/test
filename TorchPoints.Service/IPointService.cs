using System;
using System.Collections.Generic;
using TorchPoints.Core;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{
    public interface IPointService
    {
        void InsertCustomerPoint(CustomerPoints customerPoints);
        void UpdateCustomerPoint(CustomerPoints customerPoints);
        void DeleteCustomerPoint(CustomerPoints customerPoints);
        void DeleteCustomerPointsByIds(int[] ids);
        CustomerPoints GetCustomerPointsbyId(int id);
        IEnumerable<CustomerPoints> GetAllCustomerPoints(int customerId = 0);

        #region 会员积分历史操作
        PointHistory InsertPointHistory(PointHistory history);
        void UpdatePointHistory(PointHistory history);
        void DeletePointHistory(PointHistory history);
        PointHistory GetPointHistorybyId(int id);
        IEnumerable<PointHistory> GetAllPointHistory(out int totalCount, int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion
    }
}
