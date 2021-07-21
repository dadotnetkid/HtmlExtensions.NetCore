using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Controllers
{
    public class IconController : Controller
    {
        public FileStreamResult Index(string iconName)
        {
            var assembly = typeof(HtmlExtensionCore).Assembly;

            var res = assembly.GetManifestResourceNames().ToList();
            var ass = res.FirstOrDefault(x => x.Contains(iconName));
            ;
            var stream = assembly.GetManifestResourceStream(ass);
            return new FileStreamResult(stream, "image/png");
        }
    }
}
