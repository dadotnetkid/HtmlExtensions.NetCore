using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using HtmlExtensions.Core.DataGrid;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HtmlExtensions.Core.DataSourceExtension
{

    public static class ToDataTableExtension
    {
        public static DataTable ToDataTable(this DataTableGridSetting setting)
        {
            DataTableExpressionModel.HttpContext = setting.HttpContext;
            var _list = InternalListToDataTable(setting.IQueryableSource);
            var data = _list.Paginate(setting.HttpContext);
            return new DataTable()
            {
                draw = setting.HttpContext.Request.Query["draw"].Any()
                    ? Convert.ToInt32(setting.HttpContext.Request.Query["draw"][0])
                    : 1,
                data = data,
                recordsFiltered = _list.Count() ,
                recordsTotal = _list.Count() 
            };


        }
        internal static IQueryable InternalListToDataTable(IQueryable list)
        {
            if (string.IsNullOrEmpty(DataTableExpressionModel.OrderBy))
                return ListToDataTable(list, DataTableExpressionModel.SearchValue);
            return ListToDataTable(list, DataTableExpressionModel.SearchValue).OrderBy(DataTableExpressionModel.OrderBy + " " + DataTableExpressionModel.Direction);
        }
        internal static IQueryable Paginate(this IQueryable list, HttpContext httpContext)
        {
            var start = httpContext.Request.Query["start"].Any() ? Convert.ToInt32(httpContext.Request.Query["start"][0]) : 10;
            return list.Take(start + 10).Skip(start);
        }
        internal static IQueryable ListToDataTable(IQueryable list, string term)
        {

            var fields = list.ElementType.GetProperties()
                .Where(x => x.PropertyType == typeof(string))
                .ToArray();

            if (term.Contains("name"))
            {
                var data = JsonConvert.DeserializeObject<List<FilterSearchModel>>(term)?.Where(x => x.Value != "");
                if (data != null)
                    foreach (var d in data)
                    {
                        list = list.Where(d.Name + ".Contains(@0)", d.Value);
                    }
                return list.AsQueryable();
            }
            else
            {
                string filterString = string.Join(" || ", fields.Select(x => $"{x.Name}.Contains(@0)"));
                if (string.IsNullOrEmpty(term))
                    return list;
                return list.AsQueryable().Where(filterString, term);
            }



        }
        internal class DataTableExpressionModel
        {
            internal static HttpContext HttpContext { get; set; }
            internal static string SearchValue => HttpContext.Request.Query["search[value]"].Any() ? HttpContext.Request.Query["search[value]"][0] : "";

            internal static string ColumnOrder => HttpContext.Request.Query["order[0][column]"].Any()
                ?
                 HttpContext.Request.Query["order[0][column]"][0] : "";

            internal static string Direction =>
                HttpContext.Request.Query["order[0][dir]"][0] == "asc" ? "ascending" : "descending";

            internal static string OrderBy => HttpContext.Request.Query["columns[" + ColumnOrder + "][data]"].Any() ? HttpContext.Request.Query["columns[" + ColumnOrder + "][data]"][0] : "";
        }
    }


    public class DataTable
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IQueryable data { get; set; }
    }

    public class FilterSearchModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}