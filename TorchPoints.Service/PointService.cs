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
using TorchPoints.Core.Domain.Enum;

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


        public void InsertCustomerPoint(CustomerPoints info, IDbTransaction transaction = null)
        {
            info.UpdateDate = CommonHelper.GetDateTimeNow();
            _SqlDB.Insert(info, transaction);

        }
        public void UpdateCustomerPoint(CustomerPoints info, IDbTransaction transaction = null)
        {
            _SqlDB.Update(info, transaction);
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
            return _SqlDB.Get<CustomerPoints>(id);
        }
        public virtual CustomerPoints GetCustomerPointsbyCustomerId(int customerId)
        {
            var sql = new StringBuilder(@"select * from [CustomerPoints] with(nolock)");

            if (customerId > 0)
            {
                sql = sql.AppendFormat(" where customerid={0}", customerId);
            }
            var result = _SqlDB.Query<CustomerPoints>(sql.ToString());
            return result.FirstOrDefault();
        }
        public IEnumerable<CustomerPoints> GetAllCustomerPoints(int customerId = 0)
        {
            var sql = new StringBuilder(@"select * from [CustomerPoints] with(nolock)");

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
            var setting = _settingService.GetSettingByName("Point.ExpiredDate");
            int expiredDays = 30;
            if (setting != null)
            {
                int.TryParse(setting.AttributeValue, out expiredDays);
            }
            var customerpoint = GetAllCustomerPoints(history.CustomerId);
            using (IDbConnection myConnection = _SqlDB.Connection)
            {
                myConnection.Open();
                IDbTransaction tran = myConnection.BeginTransaction();
                try
                {
                    history.RemainAmount = history.Amount;
                    history.ExpiredDate = history.GetTime.AddDays(expiredDays);
                    history.Id = myConnection.Insert(history, tran)??0;
                    if (customerpoint == null || !customerpoint.Any())
                    {
                        var point = new CustomerPoints()
                        {
                            CustomerId = history.CustomerId,
                            Amount = history.Amount,
                            UpdateDate=CommonHelper.GetDateTimeNow()
                        };
                        myConnection.Insert(point, tran);
                    }
                    else
                    {
                        var point = customerpoint.FirstOrDefault();
                        point.Amount += history.Amount;
                        myConnection.Update(point, tran);
                    }
                    tran.Commit();
                    return history;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return null;
                }
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
            return _SqlDB.Get<PointHistory>(id);
        }

        public virtual IPagedList<PointHistory> GetAllPointHistory(int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var par = new Dictionary<string, object>();
            var queryFeilds = " * ";
            var from = " from [PointHistory] with(nolock)";
            var where = " where 1=1";
            if (customerId > 0)
            {
                where = where + " and customerid=@CustomerId ";
                par.Add("CustomerId", customerId);
            }

            var orderby = " order by Id desc";
            return _SqlDB.GetPaged<PointHistory>(queryFeilds, from, where, orderby, pageIndex, pageSize, par);

        }
        /// <summary>
        /// 获取有效积分记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<PointHistory> GetUnUsedPointHistory(int customerId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            List<int> statusId = new List<int>() { (int)PointStatus.NoUsed, (int)PointStatus.PartialUsed };
            var par = new Dictionary<string, object>();
            var queryFeilds = " Id,Amount,RemainAmount,CustomerId,UsedDate,StatusId,GetTime,ExpiredDate,TypeId ";
            var from = " from [PointHistory] with(nolock)";
            var where = " where StatusId in @StatusId ";
            par.Add("StatusId", statusId);
            if (customerId > 0)
            {
                where = where + " and customerid=@CustomerId ";
                par.Add("CustomerId", customerId);
            }

            var orderby = " order by [GetTime] ";
            return _SqlDB.GetPaged<PointHistory>(queryFeilds, from, where, orderby, pageIndex, pageSize, par);

        }

        /// <summary>
        /// 获取过期积分记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<ExpiresPoint> GetExpiredPointHistory(
            int customerId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var par = new Dictionary<string, object>();
            var queryFeilds = $"  Id,PointHistoryId,Amount,CustomerId,ExpireDate,CreateDate,RemainAmount ";
            var from = " from [ExpiresPoint] with(nolock)";
            var where = " where 1=1";
            if (customerId > 0)
            {
                where = where + " and customerid=@CustomerId ";
                par.Add("CustomerId", customerId);
            }
            var orderby = " order by [ExpireDate] ";
            return _SqlDB.GetPaged<ExpiresPoint>(queryFeilds, from, where, orderby, pageIndex, pageSize, par);

        }
        /// <summary>
        /// 迁移过期积分
        /// </summary>
        public virtual void MigratePoints()
        {
            var setting = _settingService.GetSettingByName("Point.ClearCount");
            int clearCount = 1000;
            if (setting != null)
            {
                int.TryParse(setting.AttributeValue, out clearCount);
            }
            List<int> statusId = new List<int>() { (int)PointStatus.NoUsed };
            var par = new Dictionary<string, object>();
            var sql = new StringBuilder(@" insert into ExpiresPoint ");
            sql.AppendFormat(@"  select top {0} Id,CustomerId,Amount,ExpiredDate,getdate(),RemainAmount from [PointHistory] with(nolock)
                 where StatusId in @StatusId and  ExpiredDate < format(getdate(), 'yyyy-MM-dd') ", clearCount);
            par.Add("StatusId", statusId);
            _SqlDB.Execute(sql.ToString(), par);

            var deleteSql = @" delete s from PointHistory s  where exists (select 1 from ExpiresPoint ep with(nolock) where  ep.PointHistoryId=s.Id)";
            _SqlDB.Execute(deleteSql.ToString(), new Dictionary<string, object>());
        }
        #endregion
    }
}
