using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PTS.Startup))]
namespace PTS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
