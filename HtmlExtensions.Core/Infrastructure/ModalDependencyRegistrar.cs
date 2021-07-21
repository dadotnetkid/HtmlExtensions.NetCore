using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.Modal;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Infrastructure
{
    public static class ModalDependencyRegistrar
    {
        public static MarkUPService UsePopUpModal(this MarkUPService service)
        {
            service.Services.AddScoped<IModalHtmlRenderer, ModalHtmlRenderer>();
            service.Services.AddScoped<IModalScriptRenderer, ModalScriptRenderer>();
            service.Services.AddScoped<IPopUpModal, PopUpModal>();
            return service;
        }
    }
}
