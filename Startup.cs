using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebDatVeMayBay.Startup))]
namespace WebDatVeMayBay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
