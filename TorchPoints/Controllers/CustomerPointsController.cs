using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TorchPoints.Model;
using TorchPoints.Service;

namespace TorchPoints.Controllers
{
    /// <summary>
    /// 积分余额
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CustomerPointsController : ControllerBase
    {       
        private readonly IPointService _pointService;
        public CustomerPointsController(
            IPointService pointService)
        {
            _pointService = pointService;
        }
        /// <summary>
        /// 获取当前会员的积分余额
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        [HttpGet("{customerid}")]
        public ApiResponseModel<dynamic> Get(int customerid)
        {
            var points = _pointService.GetAllCustomerPoints(customerid);
            if (points == null)
                return new ApiResponseModel<dynamic>(Model.StatusCode.HandledFaild);
            return new ApiResponseModel<dynamic>(points);
        }

    }
}