using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.DataGrid;
using HtmlExtensions.Core.DataSourceExtension;
using HtmlExtensions.Core.ScriptLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Infrastructure
{
    public static class DependencyRegistrar
    {
        public static MarkUPService RegisterHtmlExtensionCore(this IServiceCollection service)
        {
            service.AddHttpContextAccessor();
            service.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            service.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            service.AddScoped<IAlertLogger, AlertLogger>();
            service.AddScoped<IDataTableGrid, DataTableGrid>();
            service.AddScoped<IDataTableGridHtmlRender, DataTableGridHtmlRender>();
            service.AddScoped<IDataTableGridScriptRender, DataTableGridScriptRender>();
            return new MarkUPService(){ Services=service};
        }
    }
}
