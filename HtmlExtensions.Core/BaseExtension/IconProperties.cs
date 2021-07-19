using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using IUrlHelper = Microsoft.AspNetCore.Mvc.IUrlHelper;

namespace HtmlExtensions.Core.BaseExtension
{
    public class IconProperties
    {

        public static string GetUrl(HttpContext httpContext, string IconName)
        {
            var accessor= httpContext.RequestServices.GetRequiredService<IActionContextAccessor>();
            var urlHelper= httpContext.RequestServices.GetRequiredService<IUrlHelper>();
            var url = urlHelper;
            return url.Action("Index", "Icon", new { iconName = IconName });
        }

    }
}
