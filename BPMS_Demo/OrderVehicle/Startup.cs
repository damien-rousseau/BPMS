using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrderVehicle.Startup))]
namespace OrderVehicle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
