//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using PosAndMore.SuperAdminUI.Services;
using RepoDb;
using Scalar.AspNetCore;
using System.Reflection.Metadata.Ecma335;
using System.Text;


 
var builder = WebApplication.CreateBuilder(args);

GlobalConfiguration
   .Setup()
   .UseSqlServer();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    var jwt = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwt["SecretKey"]!);

    x.RequireHttpsMetadata = false; // prod'da true yap
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, context, cancellationToken) =>
    {
        doc.Info.Title = "PosAndMore Api Doc";
        doc.Info.Description = "PosAndMore Solutions";
        doc.Info.Version = "v1";
        doc.Info.Contact = new()
        {
            Name = "PosAndMore",
            Email = "ergul.bozok@email.com",
            Url=new Uri("https://google.com")
        };
        return Task.CompletedTask;
    });
});


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();   // → /scalar/v1 adresinde çok şık bir UI açar
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();