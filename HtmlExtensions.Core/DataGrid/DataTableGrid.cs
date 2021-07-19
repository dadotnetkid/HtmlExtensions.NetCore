using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.DataSourceExtension;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HtmlExtensions.Core.DataGrid
{
    public class DataTableGrid
    {
        public MarkUP MarkUP { get; set; }

        public void Render(DataTableGridSetting setting)
        {
            DataTableGridEvent events = new DataTableGridEvent(setting);

            if (setting.HttpContext.Request.Query["draw"].Any())
            {
                RenderDataSource(setting);
            }
            else if (events.IsEditing(setting, events))
            {
                events.RenderCreateUpdateChangesPrototype.Invoke();
            }
            else
            {
                RenderHtml(setting);
                RenderScript(setting);

                events.RenderEditorClickEvent.Invoke();
            }
        }



        private async void RenderDataSource(DataTableGridSetting setting)
        {
            await setting.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(setting.ToDataTable()));
        }

        private void RenderScript(DataTableGridSetting setting)
        {
            var writer = setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("var " + setting.Name + "dataTable;var " + setting.Name)
                .WriteLine("$(document).ready(function(){")
                .WriteLine(InitializeDataTableGrid(setting))
                .WriteLine("})")
                .WriteLine("</script>");
        }

        private string InitializeDataTableGrid(DataTableGridSetting setting)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(setting.Name + "=$('#" + setting.Name + "data')")
                .AppendLine(setting.Name + "dataTable=" + setting.Name + ".DataTable({")
                .AppendLine("dom:'ltipr',")
                .AppendLine("idSrc:  '" + setting.KeyField + "',")
                .AppendLine("orderCellsTop: true,")
                .AppendLine("\"processing\": true,")
                .AppendLine("\"serverSide\": true,")
                .AppendLine("\"ajax\":'" + setting.CallbackRoute + "',")
                .AppendLine("'columns':[");
            RenderColumnDefinition(setting, stringBuilder);
            stringBuilder.AppendLine("],")

             .AppendLine("})")
             ;
            return stringBuilder.ToString();
        }

        private void RenderColumnDefinition(DataTableGridSetting setting, StringBuilder stringBuilder)
        {
            foreach (var i in setting.Columns.DataTableColumns)
            {
                stringBuilder.AppendLine("{'data':'" + i.Name + "'},");
            }

            RenderCommandColumn(setting, stringBuilder);
        }

        private void RenderCommandColumn(DataTableGridSetting setting, StringBuilder stringBuilder)
        {
            if (setting.EnableCommandColumn)
            {
                var deleteIcon = "";
                var editIcon = "";
                if (setting.EnableEdit)
                    editIcon = "<a class=\"edit-btn\" href=\"#\"><img src=\"" +
                               IconProperties.GetUrl(setting.HttpContext, "editicon") + "\"/></a>";
                if (setting.EnableDelete)
                    deleteIcon = "<a class=\"delete-btn\" href=\"#\"><img src=\"" +
                                 IconProperties.GetUrl(setting.HttpContext, "deleteicon") + "\"/></a>";
                var icon = editIcon + deleteIcon;
                var content = "{'data':null,'className':'dt-center editor-edit-delete','defaultContent': '" + icon + "','orderable':false},";
                stringBuilder.AppendLine(content);
            }
        }
        private void RenderRowFilterColumn(DataTableGridSetting dataTableSetting)
        {
            var writer = dataTableSetting.Writer;
            writer.WriteLine("<tr id='" + dataTableSetting.Name + "-row-filter-container'>");
            foreach (var i in dataTableSetting.Columns.DataTableColumns)
            {
                writer.WriteLine($"<th style=''>");
                var type = "text";
                writer.WriteLine("<input name='" + i.Name + "'  type='" + type + "' placeholder='' data-column='" + i.Name +
                                 "' class='form-control filter-input' style='width:100% important' onkeyup='dataTableFilterByColumn(this)'/>");
                writer.WriteLine("</th>")
                    ;
            }

            if (dataTableSetting.EnableCommandColumn)
                writer.WriteLine($"<th ></th>");

            writer.WriteLine("</tr>");
            RenderRowFilterColumnScript(dataTableSetting);
        }
        private void RenderRowFilterColumnScript(DataTableGridSetting dataTableSetting)
        {
            var writer = dataTableSetting.Writer;
            writer.WriteLine("<script>")
                .WriteLine("function dataTableFilterByColumn(control){")
                .WriteLine("var search='';")
                .WriteLine("console.log($(control).val());")
                .WriteLine("if($(control).val()===''){")
                .WriteLine("search=''")
                .WriteLine("}")
                .WriteLine("else")
                .WriteLine("{")
                .WriteLine("search=$(control).attr('data-column')+':'+control.value")

                .WriteLine("}")
                .WriteLine("var data=$('#" + dataTableSetting.Name + "-row-filter-container .filter-input').serializeArray()")
                .WriteLine(dataTableSetting.Name + "dataTable.search(JSON.stringify(data)).draw()")
                .WriteLine("}")
                .WriteLine("</script>");
        }
        private void RenderHtml(DataTableGridSetting setting)
        {
            var writer = setting.Writer;
            writer.WriteLine("<table id=\"" + setting.Name + "data\" class=\"display\" style=\"width:100%\">");

            writer.WriteLine("<thead>")
                .WriteLine($"<tr>");
            ;

            foreach (var i in setting.Columns.DataTableColumns)
            {
                writer.WriteLine($"<th>{i.Caption}</th>");
            }

            if (setting.EnableCommandColumn)
                writer.WriteLine("<th></th>");

            writer.WriteLine("</tr>");
            RenderRowFilterColumn(setting);
            writer.WriteLine("</thead>").WriteLine("</table>");
        }
    }
}
