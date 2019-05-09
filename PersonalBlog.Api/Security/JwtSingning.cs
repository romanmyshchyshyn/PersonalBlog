using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PersonalBlog.Api.Security
{
    static public class JwtSingning
    {
        private const string _secretKey = "abcdefghigklmnopqrstuvwxyz";
        public static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    }
}
