using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.ScriptLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
namespace HtmlExtensions.Core.DataGrid
{
    public class DataTableGridEvent
    {
        private readonly DataTableGridSetting _setting;
        private readonly IAlertLogger _alertLogger;

        public DataTableGridEvent(DataTableGridSetting setting)
        {
            _setting = setting;
            _alertLogger = setting.HttpContext.RequestServices.GetRequiredService<IAlertLogger>();
        }

        public Action RenderEditorClickEvent => OnEditorEditClick;
        public Action RenderEditorDeleteClickEvent => OnEditorDeleteClick;
        public Action RenderEditorAddClickEvent => OnEditorAddNewClick;

        private void OnEditorAddNewClick()
        {
            if (!_setting.EnableAdd)
            {
                return;
            }

            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("$(document).ready(function(){")
                .WriteLine("$('#" + _setting.Name + "data').on('click', 'th.editor-edit-delete>.add-btn', function (e) {")
                .WriteLine("e.preventDefault();")
                .WriteLine("$('." + _setting.Name + "-datatable-child-row').remove()")
                .WriteLine("var tableTbody=$('#" + _setting.Name + "data tbody')")
                .WriteLine("tableTbody.prepend('<tr id=\"container-" + _setting.Name + "\" class=\"" + _setting.Name + "-datatable-child-row\"><td  class=\"" + _setting.Name + "-datatable-child-row\" colspan=\"" + (_setting.Columns.DataTableColumns.Count() + 1) + "\"></td></tr>')")
                .WriteLine("var tr = $('#container-" + _setting.Name + "');")
                .WriteLine("   var row = " + _setting.Name + "dataTable.row( tr );")
                .WriteLine("PerformAddNewCallback(row)")
                .WriteLine("})")
                .WriteLine("})")
                .WriteLine("</script>")
                ;
            PerformAddNewCallback();
        }

        private void PerformAddNewCallback()
        {
            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("function PerformAddNewCallback(container){")
                .WriteLine("$.post('" + _setting.CallbackRoute +
                           "?AddNew=true',function(xhr){")
                .WriteLine("$('#container-" + _setting.Name + ">td').html(xhr)")

                /*.WriteLine("$('#container-" + megamindExtension.Name + "').html(xhr)")*/
                .WriteLine("})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;
        }

        private void OnEditorDeleteClick()
        {
            if (!_setting.EnableDelete)
            {
                return;
            }

            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("$(document).ready(function(){")
                .WriteLine("$('#" + _setting.Name + "data').on('click', 'td.editor-edit-delete>.delete-btn', function (e) {")
                .WriteLine("e.preventDefault();")
                .WriteLine("var tr = $(this).closest('tr');")
                .WriteLine("var data=" + _setting.Name + "dataTable.row(tr).data();")
                .WriteLine("$('." + _setting.Name + "-datatable-child-row').remove()")
                .WriteLine("   var row = " + _setting.Name + "dataTable.row( tr );")
                .WriteLine("PerformDeleteCallback(data,row,tr)")
                .WriteLine("})")
                .WriteLine("})")
                .WriteLine("</script>")
                ;
            PerformDeleteCallback();
        }

        private void PerformDeleteCallback()
        {
            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("function PerformDeleteCallback(data,container,tr){")
                .WriteLine("$.post('" + _setting.DeleteCallbackRoute + "',{" + _setting.KeyField + ":data." + _setting.KeyField + "},function(xhr){")
                .WriteLine("tr.remove()")
                .WriteLine("})")
                .WriteLine(".always(function(){" + _setting.Name + "dataTable.ajax.reload()})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;

        }

        private void OnEditorEditClick()
        {
            if (!_setting.EnableEdit)
            {
                return;
            }

            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("$(document).ready(function(){")
                .WriteLine("$('#" + _setting.Name + "data').on('click', 'td.editor-edit-delete>.edit-btn', function (e) {")
                .WriteLine("e.preventDefault();")
                .WriteLine("var tr = $(this).closest('tr');")
                .WriteLine("var data=" + _setting.Name + "dataTable.row(tr).data();")
                .WriteLine("$('." + _setting.Name + "-datatable-child-row').remove()")
                .WriteLine("   var row = " + _setting.Name + "dataTable.row( tr );")
                .WriteLine("PerformEditCallback(data,row)")
                .WriteLine("})")
                .WriteLine("})")
                .WriteLine("</script>")
                ;
            PerformEditCallback();
        }
        private void PerformEditCallback()
        {
            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("function PerformEditCallback(data,container){")
                .WriteLine("$.post('" + _setting.CallbackRoute +
                           "?IsEditing=true',{" + _setting.KeyField + ":data." + _setting.KeyField + "},function(xhr){")
                .WriteLine("container.child.hide();")
                .WriteLine("container.child(xhr,'" + _setting.Name + "-datatable-child-row').show();")
                .WriteLine("$('." + _setting.Name + "-datatable-child-row').attr('id','container-" + _setting.Name + "')")

                /*.WriteLine("$('#container-" + megamindExtension.Name + "').html(xhr)")*/
                .WriteLine("})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;
        }

        public Action RenderCreateUpdateChangesPrototype => CreateUpdateChangesPrototype;
        public Action RenderCreatedDeleteRowProtoType => CreateDeleteRowPrototype;
        private void CreateUpdateChangesPrototype()
        {


            var callbackRoute = _setting.EditCallbackRoute;
            if (_setting.HttpContext.Request.Query["AddNew"].Any())
                callbackRoute = _setting.AddNewCallbackRoute;
            var keyField = _setting.HttpContext.Request.Form[_setting.KeyField].FirstOrDefault() ?? "";

            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("var modelData")
                .WriteLine("$.prototype.updateChanges = function () {")
                .WriteLine(" var data = $('#container-" + _setting.Name + " .markUP-editor').serializeArray();")
                .WriteLine("data[data.length]={name:'data',value:" + _setting.Name + "data}")
                .WriteLine("data[data.length]={name:'" + _setting.KeyField + "',value:'" + keyField + "'}")
                .WriteLine("$.post('" + callbackRoute + "?IsUpdate=true',data,function(xhr){")
                .WriteLine("$('.modal-backdrop').remove()")
                .WriteLine("$('#container-" + _setting.Name + "').html(xhr)")

                .WriteLine("})")
                .WriteLine(".always(function(){" + _setting.Name + "dataTable.ajax.reload()})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;
        }
        private void CreateDeleteRowPrototype()
        {
            var callbackRoute = _setting.DeleteCallbackRoute;
            var keyField = _setting.HttpContext.Request.Form[_setting.KeyField].FirstOrDefault() ?? "";

            var writer = _setting.Writer;
            writer
                .WriteLine("<script>")
                .WriteLine("var modelData")
                .WriteLine("$.prototype.deleteRow = function () {")
                .WriteLine("$.post('" + callbackRoute + "?IsDelete=true',{" + _setting.KeyField + ":" + keyField + "},function(xhr){")
                .WriteLine("})")
                .WriteLine(".always(function(){" + _setting.Name + "dataTable.ajax.reload()})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;
        }
        public bool IsUpdate => GetIsUpdate();
        public bool IsEditing => GetIsEditing();
        public bool IsDelete => GetIsDelete();
        public bool AddNew => GetIsAddNew();

        private bool GetIsAddNew()
        {
            var isEditing = _setting.HttpContext.Request.Query["AddNew"];
            return isEditing.Any();
        }


        private bool GetIsDelete()
        {
            var isEditing = _setting.HttpContext.Request.Query["IsDelete"];
            return isEditing.Any();
        }

        private bool GetIsEditing()
        {
            var isEditing = _setting.HttpContext.Request.Query["IsEditing"];
            return isEditing.Any();
        }

        private bool GetIsUpdate()
        {
            var isUpdate = _setting.HttpContext.Request.Query["IsUpdate"];
            return isUpdate.Any();
        }

        public void RenderTemplateContent()
        {
            if (_setting.TemplateContent == null)
            {
                _alertLogger.Write("No edit template created");
                return;
            }

            if (AddNew)
            {

                _setting.TemplateContent(new{ });
            }
            else
            {
                var key = _setting.HttpContext.Request.Form[_setting.KeyField][0];
                dynamic dynamicObj = JsonConvert.DeserializeObject("{'" + _setting.KeyField + "':'" + key + "'}");
                RenderCreateUpdateChangesPrototype.Invoke();
                dynamic obj = _setting.IQueryableSource.Where(_setting.KeyField + "==@0", key).FirstOrDefault();
                _setting.TemplateContent(obj);
            }

        }


    }
}
