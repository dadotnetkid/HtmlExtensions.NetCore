using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;

namespace HtmlExtensions.Core.DataGrid
{
    public class DataTableGridScriptRender: IDataTableGridScriptRender
    {
        public void RenderScript(DataTableGridSetting setting)
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
                .AppendLine("pageLength:"+setting.PageDetails.PageSize+",")
                .AppendLine("idSrc:  '" + setting.KeyField + "',")
                .AppendLine("dom:'tipr',")
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
        public void RenderRowFilterColumnScript(DataTableGridSetting dataTableSetting)
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
    }

    public interface IDataTableGridScriptRender
    {
        void RenderScript(DataTableGridSetting setting);
        void RenderRowFilterColumnScript(DataTableGridSetting dataTableSetting);
    }
}
