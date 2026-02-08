using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PosAndMore.SuperAdminUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("RestoranApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
});

builder.Services.AddScoped<ApiService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])
            ),
            ClockSkew = TimeSpan.Zero
        };

        // Tarayıcıdan gelen isteklerde cookie'dan token oku
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(context.Token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
            // 401'de login'e yönlendirme için (middleware içinde)
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/index?returnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddRazorPages(options =>
{
    // Tüm Admin area’sı → authenticated olmak zorunlu
    options.Conventions.AuthorizeAreaFolder("Admin", "/");

    // Eğer policy ile role vs. kontrolü istiyorsan
    // options.Conventions.AuthorizeAreaFolder("Admin", "/", "RequireAdminRole");

    options.Conventions.AllowAnonymousToPage("/Login");
    options.Conventions.AllowAnonymousToPage("/Index");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// UseStatusCodePages'i kaldırıyoruz – OnChallenge event'i daha doğrudan çalışıyor
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();