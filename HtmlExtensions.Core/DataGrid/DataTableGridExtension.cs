using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlExtensions.Core.DataGrid
{
    public static class DataTableGridExtension
    {
        public static DataTableGridSetting DataGrid(this MarkUP markUP, Action<DataTableGridSetting> setting)
        {
            var _setting = new DataTableGridSetting();
            setting(_setting);
            _setting.MarkUP = markUP;
            return _setting;
        }

        public static DataTableGridSetting BindToEF(this DataTableGridSetting setting, IQueryable model)
        {
            setting.IQueryableSource = model;
            return setting;
        }

        public static void Render(this DataTableGridSetting setting)
        {
            var grid = setting.HttpContext.RequestServices.GetRequiredService<IDataTableGrid>();
            grid.Render(setting);
        }
    }

    public class DataTableGridSetting : BaseSetting
    {
        public string KeyField { get; set; }
        internal IQueryable IQueryableSource { get; set; }
        public DataTableColumnCollection Columns { get; set; } = new();
        public string CallbackRoute { get; set; }
        public bool EnableAdd { get; set; }
        public bool EnableEdit { get; set; }
        public bool EnableDelete { get; set; }
        public bool EnableCommandColumn { get; set; }
        public string EditCallbackRoute { get; set; }
        public string AddNewCallbackRoute { get; set; }

        public void SetTemplateContent(Action<dynamic> content)
        {
            this.TemplateContent = content;
        }
        internal Action<dynamic> TemplateContent { get; set; }
        public PageDetails PageDetails { get; set; } = new();
    }

    public class PageDetails
    {
        public int PageSize { get; set; } = 10;
    }

    public class DataTableColumns
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public DataTableColumnProperties Properties { get; set; } = new();
    }
    public class DataTableColumnProperties
    {
        public string Width { get; set; }
    }
    public class DataTableColumnCollection
    {
        public List<DataTableColumns> DataTableColumns { get; set; } = new();
        public void Add(Action<DataTableColumns> column)
        {
            var _column = new DataTableColumns();
            column(_column);
            DataTableColumns.Add(_column);
        }
    }


}
