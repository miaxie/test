using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TorchPoints.Core;
using TorchPoints.Service;

namespace TorchPoints.Controllers
{
    /// <summary>
    /// 天气
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPointService _pointService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IPointService pointService)
        {
            _logger = logger;
            _pointService = pointService;
        }
        /// <summary>
        /// 获取Json列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            CustomerPoints t = new CustomerPoints()
            {
                CustomerId = 2,
                Amount = 100
            };
            _pointService.InsertCustomerPoint(t);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();

        }
      
    }
}
