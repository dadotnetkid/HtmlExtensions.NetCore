using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace HtmlExtensions.Core.BaseExtension
{
    public static class ViewComponentExtension
    {
        public static  void WriteLine(this MarkUP markUP, IHtmlContent content)
        {
            markUP.HtmlHelper.ViewContext.Writer.Write(content);
        }
    }
}
