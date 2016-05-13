using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LaTuerca.Startup))]
namespace LaTuerca
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
