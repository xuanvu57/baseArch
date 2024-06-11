using BaseArch.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BaseArch.Infrastructure.Identity.Registrations
{
    /// <summary>
    /// Extention to register identity services
    /// </summary>
    public static class IdentityRegistration
    {
        /// <summary>
        /// Configuration section for Jwt
        /// </summary>
        private const string JwtSection = "Identity:Jwt";

        /// <summary>
        /// Configuration section for Google Single sign-on
        /// </summary>
        private const string GoogleSsoSection = "Identity:GoogleSso";

        /// <summary>
        /// Register identity services
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterIdentity(this IServiceCollection services)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtSection)
                .ValidateOnStart();
            services.AddOptions<GoogleSsoOptions>()
                .BindConfiguration(GoogleSsoSection);

            var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Value.ValidIssuer,
                        ValidAudience = jwtOptions.Value.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecrectKey)),
                    };
                });
        }
    }
}
