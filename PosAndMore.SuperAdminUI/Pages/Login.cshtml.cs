using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PosAndMore.SuperAdmin.Models;
using System.Security.Cryptography;
using System.Text;

namespace PosAndMore.SuperAdminUI.Pages
{
    public class LoginModel : PageModel
    {
   
        private readonly ApiService _apiService;
        public LoginModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public void OnGet()
        {
        }
        [HttpPost]
        public async Task<IActionResult> OnPost(AppUser appUser)
        {
            string hash = appUser.PasswordHash;
            var response = await _apiService.PostAsync<AppUser,AppUser>(
    "AuthService/login",
    new AppUser { Username=appUser.Username,PasswordHash= appUser.PasswordHash }
);

            if (response.IsSuccess)
            {
                // response.Data garanti dolu (TResponse)

                // işlem devam
            }
            else
            {
                // Hata yönetimi süper kolay
                if (response.StatusCode == 400 && response.Errors?.ContainsKey("Password") == true)
                {
                    // Validation hatası: "Password" alanı için mesajlar
                    ModelState.AddModelError("Password", string.Join(", ", response.Errors["Password"]));
                }
                else
                {
                    // Genel hata
                    TempData["Error"] = response.ErrorMessage;
                } 
            }
            return Page();
        }
    }
}
