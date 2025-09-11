using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();

//needs import Microsoft.AspnetCore.Authentication.Core and Microsoft.AspnetCore.Authentication.Abstractions
//Pega o token enviado no header e testa para ver se e valido
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var token = builder.Configuration["TokenKey"]
            ?? throw new Exception("Token not found - program.cs");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(token)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }

    );
var app = builder.Build();

app.MapControllers();
app.UseCors(x =>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200/"));
app.UseAuthentication();
app.UseAuthorization();

app.Run();
