using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Controllers
{
    public class StyleSheetController : Controller
    {
        public IActionResult Index(string styleSheetName)
        {
            var assembly = typeof(HtmlExtensionCore).Assembly;
            var res = assembly.GetManifestResourceNames().Where(x => x.Contains(".css")).ToList();
            var ass = res.FirstOrDefault(x => x.ToLower().Contains(styleSheetName.ToLower()));
            var stream = assembly.GetManifestResourceStream(ass);
            return new FileStreamResult(stream, "text/css");
        }
    }
}
