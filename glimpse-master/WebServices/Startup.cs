using Microsoft.Owin;
using Owin;
using WebServices;
using Serilog;
using System;


[assembly: OwinStartup(typeof(Startup))]

namespace WebServices
{
    public partial class Startup
    {
       public Startup()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\logs--"+ string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + ".log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",shared:true)
               .CreateLogger();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}