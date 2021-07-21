using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Editors
{
    public class TextBox: ITextBox
    {
        public void Render(TextBoxSetting setting)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='form-inline'>")
                .AppendLine("<label for='"+setting.Name+$"'>{setting.DisplayProperties.Label}</label>")
                .AppendLine("<input type='"+(setting.IsPassword ? "password" : "text") +$"' class='form-control markUP-editor' value='"+setting.Value+"' name='"+setting.Name+"' id='"+setting.Name+"' style='margin-left:10px'/>")
                .AppendLine("</div>")
                ;
            setting.Writer.WriteLine(sb.ToString());
        }
    }

    public interface ITextBox
    {
        void Render(TextBoxSetting textBoxSetting);
    }

    public static class TextBoxDependencyRegistrar
    {
        public static MarkUPService UseTextBoxEditor(this MarkUPService service)
        {
            service.Services.AddScoped<ITextBox, TextBox>();
            return service;
        }
    }
}
