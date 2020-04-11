using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using NLog.Web;
using NLog.LayoutRenderers;
using System.Text;
using NLog;

namespace EqidManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LayoutRenderer.Register<ElapseLayoutRenderer>("elapse");

            var webHost = WebHost.CreateDefaultBuilder(args)
                     .ConfigureAppConfiguration((hostingContext, configureDelagate) =>
                     {
                         //configureDelegate默认会按照如下顺序加载ChainedConfiguration、appsetting.*.json，Environment、CommandLine
                         configureDelagate.AddJsonFile($"appsettings.secrets.json", optional: true, reloadOnChange: true);
                     })
                     .ConfigureLogging((hostingContext, loggingBuilder) =>
                     {
                         loggingBuilder.AddConsole(x=>x.IncludeScopes = true).AddDebug();
                     })
                     .UseNLog()
                     .UseStartup<Startup>()
                     .Build();
            webHost.Run();
        }
    }

    [LayoutRenderer("elapse")]
    public class ElapseLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties["Elapse"].ToString());
        }
    }
}
