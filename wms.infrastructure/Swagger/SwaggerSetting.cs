using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using wms.infrastructure.Configurations;

namespace wms.infrastructure.Swagger
{
    public static class SwaggerSetting
    {
        public static void ConfigSwagger(this IApplicationBuilder app)
        {
            if (!AppCoreConfig.Oauth2Swagger.DisableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                    options.DocExpansion(DocExpansion.None);
                    options.EnableDeepLinking();
                    options.EnableFilter();
                    options.EnablePersistAuthorization();
                    options.EnableTryItOutByDefault();
                    options.ShowExtensions();
                    options.ShowCommonExtensions();
                    options.EnableValidator();
                    options.UseRequestInterceptor("(request) => { return request; }");
                    options.UseResponseInterceptor("(response) => { return response; }");

                    options.SwaggerEndpoint("/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
                    options.OAuthClientId(AppCoreConfig.Oauth2Swagger.ClientName);
                    options.OAuthAppName(AppCoreConfig.Oauth2Swagger.ClientName);
                    options.OAuthClientSecret(AppCoreConfig.Oauth2Swagger.ClientSecret);
                });
            }
        }
        public static void SwaggerConfig(this IServiceCollection services)
        {
            if (!AppCoreConfig.Oauth2Swagger.DisableSwagger)
            {
                services.AddEndpointsApiExplorer();

                var domainName = AppDomain.CurrentDomain.FriendlyName;
                Dictionary<string, string> swaggerScopes = new Dictionary<string, string>();
                foreach (var scope in AppCoreConfig.Oauth2Swagger.Scopes.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(scope))
                        swaggerScopes.Add(scope, $"access to {scope}");
                }

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = domainName, Description = $"Swagger {domainName}", Version = "v1" });
                    var xmlFile = $"{domainName}.xml";
                    var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{AppCoreConfig.URLConnection.IDSUrl}/connect/authorize"),
                                Scopes = swaggerScopes,
                                TokenUrl = new Uri($"{AppCoreConfig.URLConnection.IDSUrl}/connect/token"),
                            }
                        },
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Scheme = "Bearer",
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "Bearer",
                                Type = SecuritySchemeType.Http,
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new string[] { }
                        }
                    });
                    options.CustomOperationIds(e => $"{e.RelativePath}");

                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                });
            }
        }

    }
}
