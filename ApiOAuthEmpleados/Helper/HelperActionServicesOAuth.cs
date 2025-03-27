using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiOAuthEmpleados.Helper
{
    public class HelperActionServicesOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperActionServicesOAuth(IConfiguration configuration)
        {
            this.Issuer = configuration.GetValue<string>("ApiOAuthToken:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuthToken:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuthToken:SecretKey");
        }

        public SymmetricSecurityKey GetKeyToken()
        {
            //byte[] data = Convert.FromBase64String(this.SecretKey);
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = this.Issuer,
                    ValidateAudience = true,
                    ValidAudience = this.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = this.GetKeyToken(),
                    ValidateIssuerSigningKey = true
                };
            });
            return options;
        }

        public Action<AuthenticationOptions> GetAuthenticateSchema() { 
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(opt =>
            {
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}
