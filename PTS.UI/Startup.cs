using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PTS.UI.Startup))]
namespace PTS.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
