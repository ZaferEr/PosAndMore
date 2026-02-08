using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models
{
    public class LoginResponse
    {
        public AppUser User { get; set; } = null!;           // Kullanıcı bilgileri (PasswordHash'siz)
        public string Token { get; set; } = string.Empty;    // JWT token
        public int ExpiresInMinutes { get; set; }            // Token süresi (dakika)
        public DateTime ExpiresAt { get; set; }              // Opsiyonel: Tam bitiş saati (UTC)
    }
}
