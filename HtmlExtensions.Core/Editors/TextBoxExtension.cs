using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Editors
{
    public static class TextBoxExtension
    {
        public static TextBoxSetting TextBox(this MarkUP markUP, Action<TextBoxSetting> setting)
        {
            var _setting = new TextBoxSetting(markUP);
            setting(_setting);
            return _setting;
        }
        public static TextBoxSetting Bind(this TextBoxSetting setting, object value)
        {
            setting.Value = value;
            return setting;
        }

        public static void Render(this TextBoxSetting setting)
        {
            var textBox=setting.RequestServices.GetRequiredService<ITextBox>();
            textBox.Render(setting);
        }
    }

    public class TextBoxSetting : BaseEditorSetting
    {
        public TextBoxSetting(MarkUP markUP)
        {
            this.MarkUP = markUP;
        }
        public object Value { get; set; }
        public bool IsPassword { get; set; }
        public DisplayProperties DisplayProperties { get; set; } = new ();
    }

    public class DisplayProperties
    {
        public string Label { get; set; }
    }
}
