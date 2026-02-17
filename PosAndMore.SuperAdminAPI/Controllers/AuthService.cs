
using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
 
using PosAndMore.SuperAdmin.Models;
using PosAndMore.SuperAdminUI.Services;
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
            DataConnection db;
            try
            {
                  db = new DataConnection(
                new DataOptions()
                  .UseSqlServer(_connectionString));
            }
            catch (Exception ex)
            {

                throw ex;
            }
         

            try
            {
                // Kullanıcıyı username ile bul
                var users = await db.GetTable<AppUser>().Where(u => u.Username == dto.Username).ToListAsync();
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