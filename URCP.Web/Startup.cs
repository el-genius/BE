using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(URCP.Web.Startup))]

namespace URCP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {           
            ConfigureAuth(app);
        }
    }
}