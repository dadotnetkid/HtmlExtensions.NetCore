using HtmlExtension.Core.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtension.Core.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindDb _db;

        public HomeController(ILogger<HomeController> logger,NorthwindDb db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult DataTablePartial()
        {
            return PartialView(_db.Customers);
        }

     
        public IActionResult EditDataTablePartial()
        {
            throw new NotImplementedException();
        }

        public IActionResult DeleteDataTablePartial(int Id)
        {
            return PartialView("DataTablePartial", _db.Customers);
        }

        public IActionResult AddNewDataTablePartial()
        {
            throw new NotImplementedException();
        }
    }
}
