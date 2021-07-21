using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlExtensions.Core.BaseExtension;
using HtmlExtensions.Core.DataGrid;
using Microsoft.AspNetCore.Http;

namespace HtmlExtensions.Core.ScriptLogger
{
    public class AlertLogger : IAlertLogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlertLogger(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Write(string message)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script>")
                .AppendLine("$(document).ready(function(){")
                .AppendLine($"alert('{message} visit https://github.com')")
                .AppendLine("})")
                .AppendLine("</script>");
            //await _httpContextAccessor.HttpContext.Response.WriteAsync(sb.ToString());
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            await _httpContextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
        public void Write(BaseSetting setting, string message)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script>")
                .AppendLine("$(document).ready(function(){")
                .AppendLine($"alert('{message} visit https://github.com')")
                .AppendLine("})")
                .AppendLine("</script>");
            //await _httpContextAccessor.HttpContext.Response.WriteAsync(sb.ToString());
            setting.Writer.WriteLine(sb.ToString());
        }
    }

    public interface IAlertLogger
    {
        Task Write(string message);
        void Write(BaseSetting setting, string message);
    }
}
