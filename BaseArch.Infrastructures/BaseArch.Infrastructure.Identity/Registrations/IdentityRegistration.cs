using BaseArch.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BaseArch.Infrastructure.Identity.Registrations
{
    public static class IdentityRegistration
    {
        private const string JwtSection = "Identity:Jwt";
        private const string GoogleSsoSection = "Identity:GoogleSso";

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
