using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PosAndMore.SuperAdmin.Models;


namespace PosAndMore.SuperAdminUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public string? ReturnUrl { get; set; }

       
        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public LoginDto Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            // Eğer kullanıcı zaten giriş yapmışsa ana sayfaya yönlendir (opsiyonel)
            if (User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect("/Admin");
            }
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _apiService.PostAsync<LoginDto, LoginResponse>(
                "AuthService/login",
                Input
            );

            if (result.IsSuccess && result.Data?.Token != null)
            {
                // API için header'a ekle
                _apiService.SetAuthorizationHeader(result.Data.Token);

                // Tarayıcıdan gelen isteklerde middleware cookie'dan okusun diye cookie'ye yaz
                Response.Cookies.Append("jwt", result.Data.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,  // HTTPS zorunlu
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                    Path = "/"
                });

                HttpContext.Session.SetString("UserName", result.Data.User?.Username ?? "");

                TempData["SuccessMessage"] = $"Hoş geldin {result.Data.User?.Username}! 🌟";

                return RedirectToPage("/Index", new { area = "Admin" });
            }
            else
            {
                ErrorMessage = result.ErrorMessage
                    ?? result.Errors?.Values.FirstOrDefault()?.FirstOrDefault()
                    ?? "Kullanıcı adı veya şifre hatalı.";

                Input.Password = string.Empty;
                ModelState.AddModelError(string.Empty, ErrorMessage);

                return Page();
            }
        }
    }

     
}