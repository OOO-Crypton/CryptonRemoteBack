using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace CryptonRemoteBack.Extensions
{
    internal static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CryptonRemoteBackClient"
                });
                options.AddServer(new OpenApiServer { Url = "http://37.230.112.158:8080" });
                options.AddServer(new OpenApiServer { Url = "http://192.168.0.133" });


                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "<p>You need to set Bearer Token to get access to API</p>" +
                                 "<p><b>Example:</b> <br />" +
                                 "Authorization: Bearer b462e2b7-b3ba-42ca-8bae-963c56d21e18</p>",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                };

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                options.UseInlineDefinitionsForEnums();
                options.UseAllOfToExtendReferenceSchemas();

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
            });
        }
    }
}
