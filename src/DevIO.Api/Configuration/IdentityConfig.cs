using System.Text;
using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.Configuration;

public static class IdentityConfig
{
    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMessagesPT_BR>()
                .AddDefaultTokenProviders();

        #region JWT

        var appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<AppSettings>();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true, //Valida se o issuer, quem está emitindo, tem que ser o mesmo quando você emitiu o token
                IssuerSigningKey = new SymmetricSecurityKey(key), // a validação acima (ValidateIssuerSigningKey) tem que ser feita pela chave
                ValidateIssuer = true, //vai validar o issuer pelo nome
                ValidateAudience = true, //valida aonde o token é válido
                ValidAudience = appSettings.ValidoEm, //busca a audiencia configurado no appsettings
                ValidIssuer = appSettings.Emissor //busca o Emissor configurado no appsettings
            };
        });

        #endregion
    }
}