
using PosAndMore.SuperAdminUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("RestoranApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);   
});
builder.Services.AddScoped<ApiService>();

builder.Services.AddAuthentication("CookieAuth")  // İstersen CookieAuthenticationDefaults.AuthenticationScheme kullan
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login";          // ← Buraya yönlendirecek (senin login Razor Page'in)
        options.AccessDeniedPath = "/AccessDenied";  // Opsiyonel: yetki yokken buraya
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);   // Cookie süresi
        options.SlidingExpiration = true;                    // Kullanım devam ederse uzat
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS zorunlu
    });

// Authorization ekle (policy vs. istersen buraya ekleyebilirsin)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Static dosyalar için (CSS, JS vs.)

app.UseRouting();

// Authentication ve Authorization middleware'leri – SIRASI ÖNEMLİ!
app.UseAuthentication();     // ← Authentication önce!
app.UseAuthorization();      // ← Unauthorized → LoginPath'e redirect eder

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();