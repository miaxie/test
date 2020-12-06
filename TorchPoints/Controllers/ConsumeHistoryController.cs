using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorchPoints.Core;
using TorchPoints.Core.Domain;
using TorchPoints.Core.Domain.Enum;
using TorchPoints.Model;
using TorchPoints.Service;

namespace TorchPoints.Controllers
{
    /// <summary>
    /// 积分消费及消费历史查询
    /// </summary>
    [Route("[controller]")]
    public class ConsumeHistoryController : ControllerBase
    {
        private readonly IPointService _pointService;
        private readonly IConsumeHistoryService _consumeHistoryService;
        public ConsumeHistoryController(
            IPointService pointService,
            IConsumeHistoryService consumeHistoryService)
        {
            _pointService = pointService;
            _consumeHistoryService = consumeHistoryService;
        }
        /// <summary>
        /// 新增积分消费历史
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponseModel<string> Post([FromBody] ConsumeModel history)
        {
            var consumeHistory = new ConsumeHistory()
            {
                CustomerId = history.CustomerId,
                TotalAmount = history.Amount,
                ConsumDate = CommonHelper.GetDateTimeNow(),
                ConsumeTypeId = (ConsumeType)history.ConsumeTypeId,
                Remark = history.Remark
            };
            var added = _consumeHistoryService.InsertConsumeHistory(consumeHistory);
            return new ApiResponseModel<string>(added);
        }

        /// <summary>
        /// 获取积分消费历史列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public ApiResponseModel<dynamic> Get(int customerId, int pageIndex = 0, int pageSize = 15)
        {
            var list = new List<dynamic>();
            var historys = _consumeHistoryService.GetAllConsumeHistorys(customerId: customerId, pageIndex: pageIndex, pageSize: pageSize);
            foreach (var item in historys)
            {
                var point = new
                {
                    TotalAmount = item.TotalAmount,
                    ConsumeTypeName = CommonHelper.GetEnumDescription(item.ConsumeTypeId),
                    CustomerId = item.CustomerId,
                    ConsumDate = item.ConsumDate,
                    Remark = item.Remark
                };
                list.Add(point);
            }
            return new ApiResponseModel<dynamic>(list, historys.TotalCount);
        }
    }
}
