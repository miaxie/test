using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TorchPoints.Core.Domain;
using TorchPoints.Core.Domain.Enum;
using TorchPoints.Model;
using TorchPoints.Service;

namespace TorchPoints.Controllers
{
    /// <summary>
    /// 积分历史
    /// </summary>
     //[ApiController]
    [Route("[controller]")]
    public class PointhistoryController : ControllerBase
    {
        private readonly IPointService _pointService;
        public PointhistoryController(
            IPointService pointService)
        {
            _pointService = pointService;
        }

        /// <summary>
        /// 获取积分历史列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{customerid}")]
        public IActionResult Get(int customerId)
        {
            var points = _pointService.GetAllPointHistory(customerId);
            return Ok(points);
        }
        /// <summary>
        /// 根据id获取积分历史明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/users/5
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetPointHistorybyId(int id)
        {
            var point = _pointService.GetPointHistorybyId(id);
            if (point == null)
                return NotFound();
            return Ok(point);
        }
        /// <summary>
        /// 新增积分历史
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]PointHistoryModel history)
        {
            var pointHistory = new PointHistory()
            {
                Amount = history.Amount,
                TypeId = (PointSourceType)history.TypeId,
                CustomerId = history.CustomerId,
                GetTime = history.GetTime,
                StatusId = (int)PointStatus.NoUsed
            };
            var added = _pointService.InsertPointHistory(pointHistory);
            return Ok(added);
        }
    }
}