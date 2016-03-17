using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MegCiptvAccounts.Startup))]
namespace MegCiptvAccounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
