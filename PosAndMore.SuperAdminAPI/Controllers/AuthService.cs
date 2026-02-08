using Azure.Core; // gerek yok gibi görünüyor, kaldırılabilir
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PosAndMore.SuperAdmin.Models;
using PosAndMore.SuperAdminUI.Services;
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
        private readonly JwtService _jwtService;

        public AuthService(IConfiguration config, JwtService jwtService)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Default connection string eksik!");
            _jwtService = jwtService;
        }

        /// <summary>
        /// Kullanıcı girişi (JWT token döner)
        /// </summary>
        [HttpPost("login")]
        public async Task<ApiResponse<LoginResponse>> Login([FromBody] LoginDto dto)
        {
            using var c = new SqlConnection(_connectionString);
            try
            {
                // Kullanıcıyı username ile bul
                var users = await c.QueryAsync<AppUser>(u => u.Username == dto.Username);
                var user = users?.FirstOrDefault();

                if (user == null)
                {
                    return ApiResponse<LoginResponse>.Failure(401, "Kullanıcı adı veya şifre hatalı");
                }

                // Şifre doğrulaması
                bool passwordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

                if (!passwordCorrect)
                {
                    return ApiResponse<LoginResponse>.Failure(401, "Kullanıcı adı veya şifre hatalı");
                }

                // Token üret
                string token = _jwtService.GenerateToken(user);

                // PasswordHash'i client'a asla gönderme!
                user.PasswordHash = null;

                // Dönüş nesnesi (token + user bilgileri)
                var loginResponse = new LoginResponse
                {
                    User = user,
                    Token = token,
                    ExpiresInMinutes = 60 // veya appsettings'ten çekilebilir
                };

                return ApiResponse<LoginResponse>.Success(loginResponse, 200);
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponse>.FromException(ex);
            }
        }
    }
     
}