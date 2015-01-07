using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(weixin.Startup))]
namespace weixin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
