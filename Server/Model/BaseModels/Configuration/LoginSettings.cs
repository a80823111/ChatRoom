using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModels.Configuration
{
    public class LoginSettings
    {
        public static JwtLoginSettings Jwt { get; set; }
        public static string HashPasswordSalt { get; set; }
    }
    public class JwtLoginSettings
    {
        public string Issuer { get; set; }
        public string SignKey { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; }
    }
}
