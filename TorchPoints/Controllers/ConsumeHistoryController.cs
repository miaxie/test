using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorchPoints.Core;
using TorchPoints.Core.Domain;
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
        public IActionResult Post([FromBody] ConsumeModel history)
        {
            var consumeHistory = new ConsumeHistory()
            {
                CustomerId = history.CustomerId,
                TotalAmount = history.Amount,
                ConsumDate = CommonHelper.GetDateTimeNow(),
                ConsumeTypeId = history.ConsumeTypeId,
                Remark = history.Remark
            };
            var added = _consumeHistoryService.InsertConsumeHistory(consumeHistory);
            return Ok(added);
        }
    }
}
