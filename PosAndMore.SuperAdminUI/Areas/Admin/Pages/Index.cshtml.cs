using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PosAndMore.SuperAdminUI.Areas.Pages
{
    public class IndexModel : PageModel
    {
        
        public void OnGet()
        {
           
        }

        //[HttpGet("logput")]
        //public async Task<IActionResult> OnGetAsync()
        //{
        //    //HttpContext.Session.Clear();
        //    //_apiService.SetAuthorizationHeader(null);
        //    //return RedirectToPage("/Login");
        //}
    }
}
