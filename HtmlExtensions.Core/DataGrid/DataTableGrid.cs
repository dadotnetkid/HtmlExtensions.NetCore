using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.DataSourceExtension;
using HtmlExtensions.Core.ScriptLogger;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HtmlExtensions.Core.DataGrid
{
    
    public class DataTableGrid:IDataTableGrid
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataTableGridHtmlRender _dataTableGridHtmlRender;
        private readonly IDataTableGridScriptRender _dataTableGridScriptRender;
        private readonly IAlertLogger _alertLogger;

        public DataTableGrid(IHttpContextAccessor httpContextAccessor,IDataTableGridHtmlRender dataTableGridHtmlRender,IDataTableGridScriptRender dataTableGridScriptRender,IAlertLogger alertLogger)
        {
           
            _httpContextAccessor = httpContextAccessor;
            _dataTableGridHtmlRender = dataTableGridHtmlRender;
            _dataTableGridScriptRender = dataTableGridScriptRender;
            _alertLogger = alertLogger;
        }

        public void Render(DataTableGridSetting setting)
        {
            if (string.IsNullOrEmpty(setting.KeyField))
            {
                _alertLogger.Write(setting,"No defined Key Field for DataTableGrid");
                return;
            }
            DataTableGridEvent events = new DataTableGridEvent(setting);

            if (setting.HttpContext.Request.Query["draw"].Any())
            {
                RenderDataSource(setting);
            }
           
            else if (events.IsEditing)
            {
                events.RenderTemplateContent();
            }
            else
            {
                _dataTableGridHtmlRender.RenderHtml(setting);
                _dataTableGridScriptRender.RenderScript(setting);

                events.RenderEditorClickEvent.Invoke();
            }
        }



        private async void RenderDataSource(DataTableGridSetting setting)
        {
            await setting.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(setting.ToDataTable()));
        }

        

        

       
       
   
     
    }
}
