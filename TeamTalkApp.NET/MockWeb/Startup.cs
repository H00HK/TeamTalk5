using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MockWeb.Startup))]
namespace MockWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
        }
    }
}
