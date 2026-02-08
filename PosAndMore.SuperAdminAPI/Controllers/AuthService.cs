
using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PosAndMore.SuperAdmin.Models;
using RepoDb;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace PosAndMore.SuperAdminAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthService : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("Default")!;
        }
        [HttpGet]
        public async Task<AppUser?> GetUserByUsername(string username)
        {
            //using var connection = new SqlConnection(_connectionString);
            //var parameters = new { Username = username };

            //// SimpleLoad ile tek kayıt çekiyoruz
            //string sql = "SELECT * FROM Users WHERE Username = @Username";
            //var users = await connection.QueryAsync<AppUser>(sql, parameters);

            //return users.FirstOrDefault();
            return null;
        }

        [HttpPost]
        public async Task<AppUser> RegisterAsync(string username, string password, string role = "Kasiyer")
        {
            //using var connection = new SqlConnection(_connectionString);
            //await connection.OpenAsync();

            //using var transaction = connection.BeginTransaction();

            //var user = new AppUser
            //{
            //    Username = username,
            //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            //    Role = role
            //};

            //// SimpleSave ile insert yapıyor
            //  connection.Create(user, transaction);

            //transaction.Commit();
            //return user;
            return null;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<AppUser>> Login([FromBody] AppUser appUser)
        {
            using var c = new SqlConnection(_connectionString);

            try
            {
                // Tek kullanıcı bekliyoruz → koleksiyondan ilkini al
                var users = await c.QueryAsync<AppUser>(u => u.Username == appUser.Username);
                var user = users?.FirstOrDefault();

                if (user == null)
                {
                    return ApiResponse<AppUser>.Failure(401, "Kullanıcı bulunamadı");
                }

                bool passwordCorrect = BCrypt.Net.BCrypt.Verify(appUser.PasswordHash, user.PasswordHash);

                if (!passwordCorrect)
                {
                    return ApiResponse<AppUser>.Failure(401, "Şifre hatalı");
                }

                user.PasswordHash = null; // client'a gönderme!

                return ApiResponse<AppUser>.Success(data: user);
            }
            catch (Exception ex)
            {
                return ApiResponse<AppUser>.FromException(ex);
            }

        }

        [HttpPost("GenerateToken")]
        public string GenerateToken([FromBody] AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
 
