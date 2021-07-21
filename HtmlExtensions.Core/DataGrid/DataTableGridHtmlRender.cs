using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.ScriptLogger;

namespace HtmlExtensions.Core.DataGrid
{
    public class DataTableGridHtmlRender : IDataTableGridHtmlRender
    {
        private readonly IDataTableGridScriptRender _dataTableGridScriptRender;
        private readonly IAlertLogger _alertLogger;

        public DataTableGridHtmlRender(IDataTableGridScriptRender dataTableGridScriptRender,IAlertLogger alertLogger)
        {
            _dataTableGridScriptRender = dataTableGridScriptRender;
            _alertLogger = alertLogger;
        }
        public void RenderHtml(DataTableGridSetting setting)
        {
           
            var writer = setting.Writer;
            writer.WriteLine("<table id=\"" + setting.Name + "data\" class=\"table table-striped table-bordered table-hover\" style=\"width:100%\">");

            writer.WriteLine("<thead>")
                .WriteLine($"<tr>");
            ;

            foreach (var i in setting.Columns.DataTableColumns)
            {
                writer.WriteLine($"<th " + (string.IsNullOrEmpty(i.Properties.Width) ?"" :"width='"+i.Properties.Width+"'") + $">{i.Caption}</th>");
            }

            if (setting.EnableCommandColumn)
            {
                writer.WriteLine("<th style='width:10%'>");
                if (setting.EnableAdd)
                    writer.WriteLine("<a href='#'  class='add-btn'><img src='" + IconProperties.GetUrl(setting.HttpContext, "addicon32") + "'/></a>");
                writer.WriteLine("</th>");
            }
            writer.WriteLine("</tr>");
            RenderRowFilterColumn(setting);
            writer.WriteLine("</thead>").WriteLine("</table>");
          
        }

        public void RenderRowFilterColumn(DataTableGridSetting dataTableSetting)
        {
            var writer = dataTableSetting.Writer;
            writer.WriteLine("<tr id='" + dataTableSetting.Name + "-row-filter-container'>");
            foreach (var i in dataTableSetting.Columns.DataTableColumns)
            {
                writer.WriteLine($"<th >");
                var type = "text";
                writer.WriteLine("<input name='" + i.Name + "'  type='" + type + "' placeholder='' data-column='" + i.Name +
                                 "' class='form-control filter-input' style='width:100% important' onkeyup='dataTableFilterByColumn(this)'/>");
                writer.WriteLine("</th>")
                    ;
            }

            if (dataTableSetting.EnableCommandColumn)
                writer.WriteLine($"<th ></th>");

            writer.WriteLine("</tr>");
            _dataTableGridScriptRender.RenderRowFilterColumnScript(dataTableSetting);
        }
    }

    public interface IDataTableGridHtmlRender
    {
        void RenderHtml(DataTableGridSetting setting);
        void RenderRowFilterColumn(DataTableGridSetting setting);
    }
}
