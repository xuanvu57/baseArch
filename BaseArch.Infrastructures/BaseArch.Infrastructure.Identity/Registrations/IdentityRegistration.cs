using BaseArch.Infrastructure.Identity.Constants;
using BaseArch.Infrastructure.Identity.Jwt;
using BaseArch.Infrastructure.Identity.Sso.Facebook;
using BaseArch.Infrastructure.Identity.Sso.Google;
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
        /// Register identity services
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterIdentity(this IServiceCollection services)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(IdentityConst.JwtSection)
                .ValidateOnStart();

            services.AddOptions<GoogleSsoOptions>()
                .BindConfiguration(IdentityConst.GoogleSsoSection);

            services.AddOptions<FacebookSsoOptions>()
                .BindConfiguration(IdentityConst.FacebookSsoSection);

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
