using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchPoints.Core;
using TorchPoints.Core.DataAccess;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{

    public class PointService : IPointService
    {
        private readonly DapperClient _SqlDB;
        private readonly ISetting _settingService;

        public PointService(IDapperFactory dapperFactory,
             ISetting settingService)
        {
            _SqlDB = dapperFactory.CreateClient("SqlServer");
            _settingService = settingService;
            //connectionString = @"Server=localhost;Database=EcPoints;User ID=sa;Password=111111; Trusted_Connection=true;";
        }
        #region 会员积分余额操作


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
            _SqlDB.Delete<CustomerPoints, int>(ids);
        }


        public virtual CustomerPoints GetCustomerPointsbyId(int id)
        {
            return _SqlDB.GetByID<CustomerPoints>(id);
        }

        public IEnumerable<CustomerPoints> GetAllCustomerPoints(int customerId = 0)
        {
            var sql = new StringBuilder(@"select * from [CustomerPoints](nolock)");

            if (customerId > 0)
            {
                sql = sql.AppendFormat(" where customerid={0}", customerId);
            }
            var result = _SqlDB.Query<CustomerPoints>(sql.ToString());
            return result;

        }

        #endregion

        #region 会员积分历史操作
        public virtual PointHistory InsertPointHistory(PointHistory history)
        {
            try
            {
                var setting = _settingService.GetSettingByName("Point.ExpiredDate");
                int expiredDays = 30;
                if (setting != null)
                {
                    int.TryParse(setting.AttributeValue, out expiredDays);
                }
                history.ExpiredDate = history.GetTime.AddDays(expiredDays);
                history.Id = _SqlDB.Insert(history);
                var customerpoint = GetAllCustomerPoints(history.CustomerId);
                if (customerpoint == null || !customerpoint.Any())
                {
                    var point = new CustomerPoints()
                    {
                        CustomerId = history.CustomerId,
                        Amount = history.Amount
                    };
                    InsertCustomerPoint(point);
                }
                else
                {
                    var point = customerpoint.FirstOrDefault();
                    point.Amount += history.Amount;
                    UpdateCustomerPoint(point);
                }
                return history;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public virtual void UpdatePointHistory(PointHistory history)
        {
            _SqlDB.Update(history);
        }
        public virtual void DeletePointHistory(PointHistory history)
        {
            _SqlDB.Delete(history);
        }
        public virtual PointHistory GetPointHistorybyId(int id)
        {
            return _SqlDB.GetByID<PointHistory>(id);
        }
        public virtual IEnumerable<PointHistory> GetAllPointHistory(int customerId = 0)
        {
            var sql = new StringBuilder(@"select * from [PointHistory](nolock)");

            if (customerId > 0)
            {
                sql = sql.AppendFormat(" where customerid={0}", customerId);
            }
            var result = _SqlDB.Query<PointHistory>(sql.ToString());
            return result;
        }
        #endregion
    }
}
