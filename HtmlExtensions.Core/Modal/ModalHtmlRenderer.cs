using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Modal
{
    public class ModalHtmlRenderer : IModalHtmlRenderer
    {
        public void RenderHtml(ModalSettings setting)
        {
            var writer = setting.Writer;
            writer.WriteLine("<div class=\"modal\" tabindex=\"-1\" role=\"dialog\" id='" + setting.Name + "'>")
                .WriteLine("  <div class=\"modal-dialog " + setting.Alignment.Vertical + " " + setting.DisplaySetting.Size.ToString() + " \" role=\"document\">")
                .WriteLine("     <div class=\"modal-content\" style='border-radius:0px !important'>")
                .WriteLine("       <div class=\"modal-header\">")
                .WriteLine("         <h5 class=\"modal-title\">" + setting.HeaderText + "</h5>")
                .WriteLine("        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">")
                .WriteLine("           <span aria-hidden=\"true\">&times;</span>")
                .WriteLine("        </button>")
                .WriteLine("      </div>")
                .WriteLine("       <div class=\"modal-body\">");
            setting.TemplateContent?.Invoke();
            writer.WriteLine("      </div>")
                .WriteLine("       <div class=\"modal-footer\">")
                .WriteLine("        <button type=\"button\" class=\"btn btn-primary\">Save changes</button>")
                .WriteLine("         <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">Close</button>")
                .WriteLine("      </div>")
                .WriteLine("   </div>")
                .WriteLine("  </div>")
                .WriteLine("</div>");



        }
    }

    public interface IModalHtmlRenderer
    {
        void RenderHtml(ModalSettings setting);
    }
}
