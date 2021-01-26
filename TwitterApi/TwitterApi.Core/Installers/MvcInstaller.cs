using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TwitterApi.DataLayer.Settings;
using TwitterApi.DataLayer.Utils;

namespace TwitterApi.Core.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            JwtSettings.Init(configuration.GetSection(nameof(JwtSettings)));

            services.AddSwaggerGen(x =>
                {
                    x.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Twitter Web Api",
                        Version = "v1"
                    });

                    //var securitySchema = new OpenApiSecurityScheme
                    //{
                    //    Description = "JWT Authorization header using the bearer schema",
                    //    Name = "Authorization",
                    //    In = ParameterLocation.Header,
                    //    Type = SecuritySchemeType.ApiKey
                    //};

                    //x.AddSecurityDefinition("Bearer", securitySchema);

                    //x.AddSecurityRequirement(new OpenApiSecurityRequirement
                    //{
                    //    {
                    //        new OpenApiSecurityScheme
                    //        {
                    //            Reference = new OpenApiReference
                    //            {
                    //                Type = ReferenceType.SecurityScheme,
                    //                Id = "Bearer"
                    //            }
                    //        },
                    //        Array.Empty<string>()
                    //    }
                    //});

                    try
                    {
                        var filePath = Path.Combine(DataUtils.SoftwareDir, "TwitterApi.Core.xml");
                        x.IncludeXmlComments(filePath);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenUtils.CreateTokenValidationParameters();
            });
        }
    }
}