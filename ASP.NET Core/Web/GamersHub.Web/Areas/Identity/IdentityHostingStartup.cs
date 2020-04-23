using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(GamersHub.Web.Areas.Identity.IdentityHostingStartup))]

namespace GamersHub.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
