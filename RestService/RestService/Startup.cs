using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestService.Startup))]
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
