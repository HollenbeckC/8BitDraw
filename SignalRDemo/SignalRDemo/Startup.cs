using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartup(typeof(SignalRDemo.Startup))]

namespace SignalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
            //GlobalConfiguration.Configuration
            //.UseSqlServerStorage("Data Source = winserv; Initial Catalog = gilmourd_db; Integrated Security = True");
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
        }
    }
}
