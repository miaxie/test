using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TorchPoints.Core;
using TorchPoints.Core.DataAccess;

namespace TorchPoints.Service
{

    public class PointService : IPointService
    {
        private readonly DapperClient _SqlDB;
        public PointService(IDapperFactory dapperFactory)
        {
            _SqlDB = dapperFactory.CreateClient("SqlServer");
            //connectionString = @"Server=localhost;Database=EcPoints;User ID=sa;Password=111111; Trusted_Connection=true;";
        }

      
        public void InsertCustomerPoint(CustomerPoints prod)
        {
            _SqlDB.Insert(prod);
           
        }
        public void UpdateCustomerPoint(CustomerPoints prod)
        {
            _SqlDB.Update(prod);
        }

        public void DeleteCustomerPoint(CustomerPoints CustomerPoint)
        {
             _SqlDB.Delete(CustomerPoint);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteCustomerPointsByIds(int[] ids)
        {
            _SqlDB.Delete<CustomerPoints,int>(ids);
        }

       
        public virtual CustomerPoints GetCustomerPointsbyId(int id)
        {
            return _SqlDB.GetByID<CustomerPoints>(id);
        }

        public IEnumerable<CustomerPoints> GetAllCustomerPoints()
        {
            var result = _SqlDB.Query<CustomerPoints>(@"select * from [CustomerPoints](nolock)");
            return result;

        }
    }
}
