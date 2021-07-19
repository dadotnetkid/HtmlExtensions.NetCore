using HtmlExtensions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Controllers
{
    public class DataTableController : Controller
    {
        private readonly ILogger<DataTableController> _logger;

        public DataTableController(ILogger<DataTableController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult DataTablePartial()
        {
            return PartialView(new List<Tests>() { new Tests() { Test = "sdfsdf" }, new Tests() { Test = "aaa" } }.AsQueryable());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult EditDataTablePartial(Tests tests)
        {
            return PartialView("DataTablePartial", new List<Tests>() { new Tests() { Test = "sdfsdf" }, new Tests() { Test = "aaa" } }.AsQueryable());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }

    public class Tests
    {
        public string Test { get; set; }
    }

}
