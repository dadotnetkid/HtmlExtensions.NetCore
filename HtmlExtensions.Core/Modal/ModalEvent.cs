using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlExtensions.Core.Modal
{
    public class ModalClientSideEvents : IModalClientSideEvents
    {
        public string OnCloseEvent { get; set; }
    }

    public interface IModalClientSideEvents
    {
        string OnCloseEvent { get; set; }
    }
}
