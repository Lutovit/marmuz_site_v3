using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(marmuz_site_v1.Startup))]
namespace marmuz_site_v1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
