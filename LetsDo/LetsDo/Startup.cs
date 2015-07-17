using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LetsDo.Startup))]
namespace LetsDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
