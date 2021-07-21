using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Modal
{
    public class ModalScriptRenderer : IModalScriptRenderer
    {
        public void RenderScript(ModalSettings setting)
        {
            var writer = setting.Writer;
            writer.WriteLine("<script>");
            writer.WriteLine("$(document).ready(function(){");
            InitializeModal(setting);
            InitializeDragModal(setting);
            RenderOnCloseEvent(setting);
            writer.WriteLine("})");
            writer.WriteLine("</script>");
        }

        private void RenderOnCloseEvent(ModalSettings settings)
        {
            var writer = settings.Writer;
            writer.WriteLine("$('#" + settings.Name + "').on('hidden.bs.modal',function(){")
                .WriteLine(settings.ClientSideEvents.OnCloseEvent)
                .WriteLine("});");
        }
        private void InitializeDragModal(ModalSettings setting)
        {
            if (!setting.AllowDragging)
                return;
            var writer = setting.Writer;
            writer.WriteLine("$('#" + setting.Name + "').draggable({")
                .WriteLine("handle: \".modal-header\"")
                .WriteLine("});")
                ;
        }

        private void InitializeModal(ModalSettings setting)
        {
            var writer = setting.Writer;
            writer.WriteLine("$('#" + setting.Name + "').modal({")
                .WriteLine("keyboard: " + (setting.CloseOnEscape ? "true" : "false") + ",")
                .WriteLine("backdrop: " + (setting.Modal ? "'static'" : "''") + ",")
                .WriteLine("show: " + (setting.ShowOnLoad ? "true" : "false") + ",")
                .WriteLine("});")
                ;
        }
    }

    public interface IModalScriptRenderer
    {
        void RenderScript(ModalSettings setting);
    }
}

