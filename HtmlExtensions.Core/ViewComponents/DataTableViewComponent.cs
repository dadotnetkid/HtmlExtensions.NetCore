using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HtmlExtensions.Core.ViewComponents
{
    [ViewComponent(Name = "DataTable")]
    public class DataTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string test)
        {
            return View("AddEditDataTablePartial", new Tests() { Test = test });
        }
    }
}
