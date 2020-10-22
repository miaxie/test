using System;
using System.Collections.Generic;
using TorchPoints.Core;

namespace TorchPoints.Service
{
    public interface IPointService
    {
        void InsertCustomerPoint(CustomerPoints customerPoints);
        void UpdateCustomerPoint(CustomerPoints customerPoints);
        void DeleteCustomerPoint(CustomerPoints customerPoints);
        void DeleteCustomerPointsByIds(int[] ids);
        CustomerPoints GetCustomerPointsbyId(int id);
        IEnumerable<CustomerPoints> GetAllCustomerPoints();
    }
}
