using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;

namespace HtmlExtensions.Core.DataGrid
{
    public interface IDataTableGrid
    {
        void Render(DataTableGridSetting setting);
    }
}
