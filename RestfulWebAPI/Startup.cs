using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RestfulWebAPI.Startup))]

namespace RestfulWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
