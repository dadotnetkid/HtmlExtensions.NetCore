using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.ScriptRenderer
{
    public static class ScriptRendererExtension
    {
        public static void GetScripts(this MarkUP mark, params Scripts[] scripts)
        {
            var url = mark.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();
            foreach (var i in scripts)
            {
                mark.Writer.WriteLine($"<script src='{url.Action("index", "script", new { scriptName = i.Extension.Script })}'></script>");
            }
        }
    }
    public static class StyleSheetRendererExtension
    {
        public static void GetStyleSheet(this MarkUP mark, params StyleSheet[] styleSheets)
        {
            var url = mark.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();
            foreach (var i in styleSheets)
            {
                mark.Writer.WriteLine($"<link href='{url.Action("index","stylesheet",new{ styleSheetName =i.Extension.StyleSheet})}' rel='stylesheet'/>");
            }
        }
    }

    public class StyleSheet
    {
        public Extension Extension { get; set; } = new();
    }

    public class Scripts
    {
        public Extension Extension { get; set; } = new();
    }

    public class Extension
    {
        public static Extension DataTableGrid => new() { Script = "DataTables.js", StyleSheet = "DataTables.css" };
        public string StyleSheet { get; set; }

        internal string Script { get; set; }
    }
}
