using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
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
            var historys = _pointService.GetAllPointHistory();
            foreach (var item in historys)
            {
                var point = new PointHistoryModel()
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    TypeName = item.TypeId.GetDisplayName(),
                    CustomerId = item.CustomerId,
                    GetTime = item.GetTime,
                    ExpiredDate = item.ExpiredDate,
                    Status = item.StatusId.GetDisplayName()
                };
                list.Add(point);
            }
            var model = new PointHistoryListModel() { Historys=list};
            return View(model);
        }
    }
}