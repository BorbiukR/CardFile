using CardFile.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardFile.WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            string connectionForMainDb = configuration.GetConnectionString("CardFileAPI");
            services.AddDbContext<CardFileDbContext>(options => options.UseSqlServer(connectionForMainDb));

            string connectionForIdentityDb = configuration.GetConnectionString("IdintityCardFileAPI");
            services.AddDbContext<CardFileIdentityDbContext>(options => options.UseSqlServer(connectionForIdentityDb));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<CardFileIdentityDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}