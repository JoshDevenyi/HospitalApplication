using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalApplication.Startup))]
namespace HospitalApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
