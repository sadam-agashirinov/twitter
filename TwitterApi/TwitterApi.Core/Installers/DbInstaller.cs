using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitterApi.DataLayer.Common;

namespace TwitterApi.Core.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TwitterDbContext>(x =>
                x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}