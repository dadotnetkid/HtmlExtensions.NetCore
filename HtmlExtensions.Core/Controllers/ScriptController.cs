using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Controllers
{
    public class ScriptController : Controller
    {
        public IActionResult Index(string scriptName)
        {
            var assembly = typeof(HtmlExtensionCore).Assembly;
            var res = assembly.GetManifestResourceNames().Where(x => x.Contains(".js")).ToList();
            var ass = res.FirstOrDefault(x => x.ToLower().Contains(scriptName.ToLower()));
            var stream = assembly.GetManifestResourceStream(ass);
            return new FileStreamResult(stream,"text/javascript");
        }
    }
}
