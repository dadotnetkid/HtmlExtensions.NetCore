using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.BaseExtension
{
    public class MarkUP
    {
        public IHtmlHelper HtmlHelper { get; set; }
        public MarkUPWriter Writer => new(HtmlHelper.ViewContext.Writer);
    }

    public class MarkUPService
    {
        public IServiceCollection Services { get; set; }
    }
    public static class MarkUPExtension
    {
        public static MarkUP MarkUP(this IHtmlHelper htmlHelper)
        {
            return new MarkUP()
            {
                HtmlHelper = htmlHelper
            };
        }
       
    }
}
