using Microsoft.IdentityModel.Tokens;
using PosAndMore.SuperAdmin.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PosAndMore.SuperAdminUI.Services // veya kendi namespace'in
{
    public class JwtService
    {
        private readonly string _secretKey;      // appsettings.json'dan gelecek
        private readonly string _issuer;         // "https://yourapi.com"
        private readonly string _audience;       // "https://yourapp.com"
        private readonly int _expiryMinutes;     // token süresi (dakika)

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"]
                ?? throw new ArgumentNullException("Jwt:SecretKey appsettings'te yok");
            _issuer = configuration["Jwt:Issuer"]
                ?? "https://yourapi.com";
            _audience = configuration["Jwt:Audience"]
                ?? "https://yourapp.com";
            _expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"] ?? "60");
        }

        /// <summary>
        /// Kullanıcı bilgileriyle JWT token üretir
        /// </summary>
        public string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Username), // email username ile aynıysa
                new Claim(ClaimTypes.Role, user.Role ?? "user"),
                // İstersen ekstra claim ekle: new Claim("CustomClaim", "değer")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Token geçerli mi? (opsiyonel, genellikle middleware'de yapılır)
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero // token süresi tam olarak bitsin
                };

                var handler = new JwtSecurityTokenHandler();
                return handler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Token'dan user ID'yi al (örnek kullanım)
        /// </summary>
        public int? GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null) return null;

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : null;
        }
    }
}