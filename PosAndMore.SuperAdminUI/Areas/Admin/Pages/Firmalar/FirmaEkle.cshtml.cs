using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PosAndMore.SuperAdmin.Models;
using PosAndMore.SuperAdmin.Models.DtoModels;
using System.Linq;
 
namespace PosAndMore.SuperAdminUI.Areas.Admin.Pages.Firmalar
{
    public class FirmaEkleModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public FirmalarDto FirmaDtoModel { get; set; }

        public List<SelectListItem> Iller { get; set; } = new();

        public FirmaEkleModel(ApiService apiService)
        {
            _apiService = apiService;
            FirmaDtoModel = new FirmalarDto();
        }

        public async Task OnGetAsync()
        {
            var iller=await _apiService.GetAsync<List<IlDto>>("api/FirmaService/GetIller");
            Iller = iller.Data.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.IlAdi
            }).ToList();

        }

       
        public async Task<IActionResult> OnGetIlcelerAsync([FromQuery(Name = "FirmaDtoModel.IlId")] int IlId)
        {
 
            var ilceler = await _apiService.GetAsync<List<IlceDto>>("api/FirmaService/GetIlceler?IlId="+ IlId.ToString());

            var html = new System.Text.StringBuilder();
            html.Append("<option value=\"\">-- İlçe Seçiniz --</option>");

            foreach (var ilce in ilceler.Data)
            {
                html.Append($"<option value=\"{ilce.Id}\">{ilce.IlceAdi}</option>");
            }

            return Content(html.ToString(), "text/html");

        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // illeri tekrar yükle
                return Page();
            }

            var result = await _apiService.PostAsync<FirmalarDto,ApiResponse<FirmalarDto>>("api/FirmaService/FirmaEkle",FirmaDtoModel);
            // Firma kaydet...
            // await db.InsertAsync(Firma);

            TempData["Success"] = "Firma eklendi!";
            return RedirectToPage("/Firma/Index");
        }
    }
}
