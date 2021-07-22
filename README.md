# HtmlExtensions.NetCore
[Nuget Package](https://www.nuget.org/packages/HtmlExtensions.Core/)
```
Install-Package HtmlExtensions.Core -Version 
```
```
In startup DI register htmlextension and other components
 services.RegisterHtmlExtensionCore().UsePopUpModal().UseTextBoxEditor();
```
```
@{
        Html.MarkUP().GetStyleSheet(new StyleSheet() { Extension = Extension.DataTableGrid });
        Html.MarkUP().GetScripts(new Scripts() { Extension = Extension.DataTableGrid });
}
```

```
Create Partial View Containing datagrid
Create Action in Controller which same name with the partial view you've created
example is 
        public PartialViewResult DataTablePartial()
        {
            /// db.customer is the entity framework return iqueryable of customer
            return PartialView(_db.Customers);
        }
        DataTablePartial.cshtml
```
```
using HtmlExtensions.Core.BaseExtension
@using HtmlExtensions.Core.DataGrid
@using HtmlExtensions.Core.Modal
@model IQueryable<Customers>
@{
    Html.MarkUP().DataGrid(settings =>
    {
        settings.Name = "datagrid1";
        settings.CallbackRoute = Url.Action("DataTablePartial");/// callbackroute to the server on pagination,searching, sorting and on editing and on adding 
        settings.EnableEdit = true;
        settings.EnableAdd = true;
        settings.EnableDelete = true;
        settings.EnableCommandColumn = true;
        settings.KeyField = "Id";
        settings.PageDetails.PageSize = 5;

        settings.Columns.Add(col =>
        {
            col.Caption = "Customer Id";
            col.Name = "CustomerID";
            col.Properties.Width = "100px";
        });
        settings.Columns.Add(col =>
        {
            col.Caption = "Contact Name";
            col.Name = "ContactName";
        });
        settings.SetTemplateContent(content =>
        {
            //where you put partialview of your editors
            //Customers is my poco for Customer table
            
            @Html.Partial("CustomerAddEditPartial,content as Customers);
            
        });
    }).BindToEF(Model).Render();
}
```
```
        Html.MarkUP().Modal(modalSettings =>
            {
                modalSettings.Name = "modal";
                modalSettings.ShowOnLoad = true;
                modalSettings.CloseOnEscape = true;
                modalSettings.Modal = true;
                modalSettings.HeaderText = content?.ContactTitle;
                modalSettings.Alignment.Vertical = ModalAlignment.VerticallyCenter;
                modalSettings.DisplaySetting.Size = ModalSize.Medium ;
                modalSettings.AllowDragging = true;
                modalSettings.ClientSideEvents.OnCloseEvent = "alert('asf')";
                modalSettings.SetTemplateContent(async() =>
                {
                   //
                  you can use partial or add content using viewcontext.writer.writeline()
                   or directly call the other editors like 
                   Html.MarkUP().TextBox(setting =>
                     {
                         setting.Name = "Contact Name";
                         setting.DisplayProperties.Label = "Contact Name";

                     }).Bind(Model?.ContactName).Render();
                     //select editors is on the way
                     // also the buttons
                });
            }).Render();
```

