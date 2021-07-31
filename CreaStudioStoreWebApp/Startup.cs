using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CreaStudioStoreWebApp.Startup))]
namespace CreaStudioStoreWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
