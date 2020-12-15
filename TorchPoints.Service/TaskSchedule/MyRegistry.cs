using System;
using System.Collections.Generic;
using System.Text;
using FluentScheduler;
using Serilog;
using TorchPoints.Core;
using TorchPoints.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace TorchPoints.Service.TaskSchedule
{
    public class MyRegistry : Registry
    {
        private readonly IServiceProvider _serviceProvider;
        public MyRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //Schedule(() =>
            //{

            //}).ToRunNow().AndEvery(1).Minutes();//.At(3, 0);
            //Schedule<Job>().ToRunEvery(1).Minutes();
            Schedule<Job>().ToRunEvery(1).Days().At(3, 0);
        }
        /// <summary>
        /// 清除失效积分
        /// </summary>
        public class Job : IJob
        {
             private readonly IPointService _pointService;
            private readonly ISetting _settingService;
            public Job(IPointService pointService,
                ISetting settingService) {
                _pointService = pointService;
                _settingService = settingService;
            }
            public void Execute()
            {
                _pointService.MigratePoints();
            }
        }


        private new Schedule Schedule<T>() where T : IJob
        {
            return Schedule(_serviceProvider.GetService<T>())
                .WithName(typeof(T).Name);
        }
    }
}
