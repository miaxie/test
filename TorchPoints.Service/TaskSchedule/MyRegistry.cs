﻿using System;
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
            //    var point = new PointHistory()
            //    {
            //        CustomerId = 10,
            //        Amount = 1,
            //        TypeId = Core.Domain.Enum.PointSourceType.Gift,
            //        RemainAmount = 1,
            //        GetTime = CommonHelper.GetDateTimeNow(),
            //        ExpiredDate = CommonHelper.GetDateTimeNow().AddDays(30),
            //        StatusId = Core.Domain.Enum.PointStatus.NoUsed
            //    };
            //    //_pointService.InsertPointHistory(point);
            //    Console.WriteLine($"当前时间{CommonHelper.GetDateTimeNow()}");

            //}).ToRunNow().AndEvery(1).Minutes();//.At(3, 0);

            Schedule<Job>().ToRunNow().AndEvery(1).Minutes();
        }

        public class Job : IJob
        {
             private readonly IPointService _pointService;
            public Job(IPointService pointService) {
                _pointService = pointService;
            }
            public void Execute()
            {
                var point = new PointHistory()
                {
                    CustomerId = 10,
                    Amount = 1,
                    TypeId = Core.Domain.Enum.PointSourceType.Gift,
                    RemainAmount = 1,
                    GetTime = CommonHelper.GetDateTimeNow(),
                    ExpiredDate = CommonHelper.GetDateTimeNow().AddDays(30),
                    StatusId = Core.Domain.Enum.PointStatus.NoUsed
                };
                _pointService.InsertPointHistory(point);
            }
        }


        private new Schedule Schedule<T>() where T : IJob
        {
            return Schedule(_serviceProvider.GetService<T>())
                .WithName(typeof(T).Name);
        }
    }
}
