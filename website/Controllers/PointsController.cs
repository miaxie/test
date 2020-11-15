using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.OpenApi.Extensions;
using TorchPoints.Core;
using TorchPoints.Core.Domain.Enum;
using TorchPoints.Service;
using website.Models;

namespace website.Controllers
{
    public class PointsController : Controller
    {
        private readonly IPointService _pointService;
        public PointsController(
            IPointService pointService)
        {
            _pointService = pointService;
        }
        public IActionResult Index()
        {
            var list = new List<PointHistoryModel>();
            var historys = _pointService.GetAllPointHistory(pageIndex: 0, pageSize: 15);
            foreach (var item in historys)
            {
                var point = new PointHistoryModel()
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    TypeName = CommonHelper.GetEnumDescription(item.TypeId),
                    CustomerId = item.CustomerId,
                    GetTime = item.GetTime,
                    ExpiredDate = item.ExpiredDate,
                    Status = CommonHelper.GetEnumDescription(item.StatusId)
                };
                list.Add(point);
            }
            var model = new PointHistoryListModel() { };
            model.Historys = list;
            model.TotalCount = historys.TotalCount;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetList(int pageIndex=0,int pageSize=15)
        {
            var list = new List<PointHistoryModel>();
            var historys = _pointService.GetAllPointHistory( pageIndex: pageIndex, pageSize: pageSize);
            foreach (var item in historys)
            {
                var point = new PointHistoryModel()
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    TypeName = CommonHelper.GetEnumDescription(item.TypeId),
                    CustomerId = item.CustomerId,
                    GetTime = item.GetTime,
                    ExpiredDate = item.ExpiredDate,
                    Status = CommonHelper.GetEnumDescription(item.StatusId)
                };
                list.Add(point);
            }
            return Json(list);
        }
    }
}