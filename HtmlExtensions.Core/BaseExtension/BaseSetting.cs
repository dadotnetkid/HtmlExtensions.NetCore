using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HtmlExtensions.Core.BaseExtension
{
    public class BaseSetting
    {

        public string Name { get; set; }
        public MarkUP MarkUP { get; set; }
        public HttpContext HttpContext => MarkUP?.HtmlHelper.ViewContext.HttpContext;
        public MarkUPWriter Writer => new (MarkUP.HtmlHelper.ViewContext.Writer);
        
    }

    public class MarkUPWriter
    {
        private readonly TextWriter _writer;

        public MarkUPWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public MarkUPWriter WriteLine(string content)
        {
            _writer.WriteLine(content);
            return new MarkUPWriter(_writer);
        }
    }
}
