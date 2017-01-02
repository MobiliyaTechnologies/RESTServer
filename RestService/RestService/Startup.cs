using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestService.Startup))]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace RestService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
