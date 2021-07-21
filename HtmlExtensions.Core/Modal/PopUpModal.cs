using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.ScriptLogger;

namespace HtmlExtensions.Core.Modal
{
    public class PopUpModal : IPopUpModal
    {
        private readonly IAlertLogger _alertLogger;
        private readonly IModalHtmlRenderer _modalHtmlRenderer;
        private readonly IModalScriptRenderer _modalScriptRenderer;

        public PopUpModal(IAlertLogger alertLogger,IModalHtmlRenderer modalHtmlRenderer,IModalScriptRenderer modalScriptRenderer)
        {
            _alertLogger = alertLogger;
            _modalHtmlRenderer = modalHtmlRenderer;
            _modalScriptRenderer = modalScriptRenderer;
        }
        public void Render(ModalSettings setting)
        {
            if (string.IsNullOrEmpty(setting.Name))
            {
                _alertLogger.Write(setting, "No defined Name for PopUpModal");
                return;
            }

            _modalHtmlRenderer.RenderHtml(setting);
            _modalScriptRenderer.RenderScript(setting);
        }
    }

    public interface IPopUpModal
    {
        void Render(ModalSettings setting);
    }
}
