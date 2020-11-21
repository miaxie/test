using System;
using System.Collections.Generic;
using System.Text;
using TorchPoints.Core;
using TorchPoints.Core.DataAccess;
using TorchPoints.Core.Domain;
using TorchPoints.Core.Domain.Enum;

namespace TorchPoints.Service
{
    /// <summary>
    /// 积分消费操作类
    /// </summary>
    public class ConsumeHistoryService : IConsumeHistoryService
    {
        private readonly DapperClient _sqlDB;
        private readonly ISetting _settingService;
        private readonly IPointService _pointService;

        public ConsumeHistoryService(IDapperFactory dapperFactory,
             ISetting settingService,
             IPointService pointService)
        {
            _sqlDB = dapperFactory.CreateClient("SqlServer");
            _settingService = settingService;
            _pointService = pointService;
        }
        /// <summary>
        /// 插入积分消费记录
        /// </summary>
        /// <param name="info"></param>
        public string InsertConsumeHistory(ConsumeHistory info)
        {
            var message = "操作成功";
            var customerPoints = _pointService.GetCustomerPointsbyCustomerId(info.CustomerId);
            if (customerPoints==null || customerPoints.Amount<info.TotalAmount)
            {
                return "积分余额不足";
            }
            info.Id = _sqlDB.Insert(info);
            //更新会员积分余额
            if (customerPoints != null)
            {
                customerPoints.Amount = customerPoints.Amount - info.TotalAmount;
                customerPoints.UpdateDate = CommonHelper.GetDateTimeNow();
                _pointService.UpdateCustomerPoint(customerPoints);
            }
            //获取有效积分历史记录，按时间排序，先消费快过期的积分
            var pointHistorys = _pointService.GetUnUsedPointHistory(customerId:info.CustomerId);
            //如果一条积分记录数量不够，则继续用下一条记录拆分
            //在明细表中记录拆分详情
            //更改积分历史表的积分状态
            var totalAmount = info.TotalAmount;
            foreach (var p in pointHistorys)
            {
                var detailAmount = 0;
                //单条积分余额大于消费积分总额
                if (p.RemainAmount>totalAmount)
                {
                    detailAmount = totalAmount;
                    p.RemainAmount = p.RemainAmount - totalAmount;
                    p.UsedDate = CommonHelper.GetDateTimeNow();
                    p.StatusId = PointStatus.PartialUsed;
                    _pointService.UpdatePointHistory(p);
                }
                else if (p.RemainAmount == totalAmount)
                {
                    detailAmount = totalAmount;
                    p.RemainAmount = p.RemainAmount - totalAmount;
                    p.UsedDate = CommonHelper.GetDateTimeNow();
                    p.StatusId = PointStatus.Used;
                    _pointService.UpdatePointHistory(p);
                }
                else
                {
                    detailAmount = p.RemainAmount;
                    p.RemainAmount = 0;
                    p.UsedDate = CommonHelper.GetDateTimeNow();
                    p.StatusId = PointStatus.Used;
                    _pointService.UpdatePointHistory(p);
                    totalAmount = totalAmount - detailAmount;
                }
                var detail = new ConsumeDetail()
                {
                    PointHistoryId = p.Id,
                    ConsumeHistoryId = info.Id,
                    Amount = detailAmount
                };
                InsertConsumeDetail(detail);
                if (totalAmount <= 0) break;
            }
            return message;
        }
        /// <summary>
        /// 插入积分消费记录明细
        /// </summary>
        /// <param name="info"></param>
        public void InsertConsumeDetail(ConsumeDetail info)
        {
            _sqlDB.Insert(info);
        }

        /// <summary>
        /// 获取积分消费历史
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<ConsumeHistory> GetAllConsumeHistorys(int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var par = new Dictionary<string, object>();
            var queryFeilds = " * ";
            var from = " from [ConsumeHistory] with(nolock)";
            var where = " where 1=1";
            if (customerId > 0)
            {
                where = where + " and customerid=@CustomerId ";
                par.Add("CustomerId", customerId);
            }

            var orderby = " order by Id desc";
            return _sqlDB.GetPaged<ConsumeHistory>(queryFeilds, from, where, orderby, pageIndex, pageSize, par);

        }
    }
}
