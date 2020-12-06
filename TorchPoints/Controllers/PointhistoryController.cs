using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TorchPoints.Core;
using TorchPoints.Core.Domain;
using TorchPoints.Core.Domain.Enum;
using TorchPoints.Model;
using TorchPoints.Service;
using TorchPoints.Service.Task;

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
        [HttpGet("{customerId}")]
        public ApiResponseModel<dynamic> Get(int customerId, int pageIndex = 0, int pageSize = 15)
        {
           
            var list = new List<dynamic>();
            var historys = _pointService.GetAllPointHistory(customerId: customerId, pageIndex: pageIndex, pageSize: pageSize);
            foreach (var item in historys)
            {
                var point = new
                {
                    Amount = item.Amount,
                    TypeName = CommonHelper.GetEnumDescription(item.TypeId),
                    CustomerId = item.CustomerId,
                    GetTime = item.GetTime,
                    ExpiredDate = item.ExpiredDate,
                    Status = CommonHelper.GetEnumDescription(item.StatusId)
                };
                list.Add(point);
            }
            return new ApiResponseModel<dynamic>(list, historys.TotalCount);
        }
        /// <summary>
        /// 根据id获取积分历史明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/users/5
        [HttpGet]
        [Route("GetById/{id}")]
        public ApiResponseModel<dynamic> GetPointHistorybyId(int id)
        {
            var point = _pointService.GetPointHistorybyId(id);
            if (point == null)
                return new ApiResponseModel<dynamic>(Model.StatusCode.HandledFaild);
            return new ApiResponseModel<dynamic>(point);
        }
        /// <summary>
        /// 新增积分历史
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponseModel<dynamic> Post([FromBody] PointHistoryModel history)
        {
            var pointHistory = new PointHistory()
            {
                Amount = history.Amount,
                TypeId = (PointSourceType)history.TypeId,
                CustomerId = history.CustomerId,
                GetTime = CommonHelper.GetDateTimeNow(),
                StatusId = (int)PointStatus.NoUsed
            };
            var added = _pointService.InsertPointHistory(pointHistory);
            if (added == null)
            {
                return new ApiResponseModel<dynamic>(Model.StatusCode.HandledFaild, "保存失败，请稍后重试");
            }
            var result = new
            {
                Amount = added.Amount,
                TypeName = CommonHelper.GetEnumDescription(added.TypeId),
                CustomerId = added.CustomerId,
                GetTime = added.GetTime
            };
            return new ApiResponseModel<dynamic>(result);
        }
    }
}