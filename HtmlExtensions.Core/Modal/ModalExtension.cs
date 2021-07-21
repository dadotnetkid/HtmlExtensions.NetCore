using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.Modal
{
    public static class ModalExtension
    {
        public static ModalSettings Modal(this MarkUP markUp, Action<ModalSettings> settings)
        {
            var _setting = new ModalSettings(markUp);
            settings(_setting);
            return _setting;
        }

        public static void Render(this ModalSettings setting)
        {
            var modal = setting.RequestServices.GetRequiredService<IPopUpModal>();
            modal.Render(setting);
        }

    }

    public class ModalSettings : BaseSetting
    {

        public ModalSettings(MarkUP markUp)
        {
            MarkUP = markUp;
        }


        public bool CloseOnEscape { get; set; }
        public bool ShowOnLoad { get; set; }
        public Action TemplateContent { get; set; }
        public string HeaderText { get; set; }
        public bool Modal { get; set; }
        public ModalAlignment Alignment { get; set; } = new();
        public DisplaySetting DisplaySetting { get; set; } = new();
        public bool AllowDragging { get; set; }
        public ModalClientSideEvents ClientSideEvents { get; set; } = new ();

        public void SetTemplateContent(Action template)
        {
            this.TemplateContent = template;
        }


    }

  

    public class DisplaySetting
    {
        public ModalSize Size { get; set; } = new();
    }

    public class ModalSize
    {
        public static ModalSize Large => new () { Size = "modal-lg" };
        public static ModalSize ExtraLarge => new () { Size = "modal-xl" };
        public static ModalSize Small => new () { Size = "modal-sm" };
        public static ModalSize Medium=> new() { Size = "modal-md" };
        private string Size { get; init; } = "modal-sm";
        public override string ToString()
        {
            return Size;
        }
    }

    public class ModalAlignment
    {
        public string Vertical { get; set; }
        public static string VerticallyCenter => "modal-dialog-centered";
    }
}
