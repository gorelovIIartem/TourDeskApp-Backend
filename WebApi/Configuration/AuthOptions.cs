using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Configuration
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "http://localhost:50990/";
        const string KEY = "securitykey123aa";
        public const int LIFETIME = 60;

        public static SymmetricSecurityKey GetSummetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
