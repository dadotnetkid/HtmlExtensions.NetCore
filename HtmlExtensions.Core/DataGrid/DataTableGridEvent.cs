using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HtmlExtensions.Core.DataGrid
{
    public class DataTableGridEvent
    {
        private readonly DataTableGridSetting _setting;

        public DataTableGridEvent(DataTableGridSetting setting)
        {
            _setting = setting;
        }

        public Action RenderEditorClickEvent => OnEditorEditClick;
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
                .WriteLine("console.log(row)")
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
                .WriteLine("console.log(data." + _setting.KeyField + ")")
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
                .WriteLine("console.log(data)")
                .WriteLine("$.post('" + callbackRoute + "?IsUpdate=true',data,function(xhr){")
                .WriteLine("$('.modal-backdrop').remove()")
                .WriteLine("$('#container-" + _setting.Name + "').html(xhr)")

                .WriteLine("})")
                .WriteLine(".always(function(){" + _setting.Name + "dataTable.ajax.reload()})")
                .WriteLine("}")
                .WriteLine("</script>")
                ;
        }

        public bool IsEditing(DataTableGridSetting setting, DataTableGridEvent events)
        {

            try
            {
                var isEditing = setting.HttpContext.Request.Query["IsEditing"];
                var addNew = setting.HttpContext.Request.Query["AddNew"];
                var isUpdate = setting.HttpContext.Request.Query["IsUpdate"];
                Debug.WriteLine("editing" + isEditing.FirstOrDefault());

                if (isEditing.Any())
                {
                    var key = setting.HttpContext.Request.Form[setting.KeyField][0];
                    dynamic dynamicObj = JsonConvert.DeserializeObject("{'" + setting.KeyField + "':'" + key + "'}");
                    setting.TemplateContent(dynamicObj);
                }


                if (isEditing.Any() || addNew.Any() || isUpdate.Any())
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
    }
}
