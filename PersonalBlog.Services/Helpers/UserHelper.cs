using System.Security.Cryptography;
using System.Text;

namespace PersonalBlog.Services.Helpers
{
    public static class UserHelper
    {
        public static string GetPasswordHash(string password)
        {
            var md5 = MD5.Create();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                hash.Append(data[i].ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
