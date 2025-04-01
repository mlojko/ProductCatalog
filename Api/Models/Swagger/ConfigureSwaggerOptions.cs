using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Api.Models.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IOptions<ApiVersioningOptions> _options;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<ApiVersioningOptions> options)
        {
            _provider = provider;
            _options = options;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"API {description.GroupName}",
                Version = description.ApiVersion.ToString()
            };

            if (description.ApiVersion == _options.Value.DefaultApiVersion)
            {
                if (!string.IsNullOrWhiteSpace(info.Description))
                {
                    info.Description += " ";
                }

                info.Description += "Default API version.";
            }

            if (description.IsDeprecated)
            {
                if (!string.IsNullOrWhiteSpace(info.Description))
                {
                    info.Description += " ";
                }

                info.Description += "This API version is deprecated.";
            }

            return info;
        }
    }
}
