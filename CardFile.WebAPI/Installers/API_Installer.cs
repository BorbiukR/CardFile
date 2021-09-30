using AutoMapper;
using CardFile.BLL.Interfaces;
using CardFile.BLL.MappingProfiles;
using CardFile.BLL.Services;
using CardFile.DAL;
using CardFile.WebAPI.Filters;
using CardFile.WebAPI.Interfaces;
using CardFile.WebAPI.MappingProfiles;
using CardFile.WebAPI.Services;
using CardFile.WebAPI.Settings;
using Data.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CardFile.WebAPI.Installers
{
    public class API_Installer : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICardFileService, CardFileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IMailService, SendGridMailService>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });

            services.AddAutoMapper(typeof(Startup));

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BLLAutomapperProfile());
                mc.AddProfile(new PLAutomapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
                    .AddFluentValidation(mvcConfiguration => 
                        mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddCors();
            services.AddControllers();
            services.AddRazorPages();
        }
    }
}