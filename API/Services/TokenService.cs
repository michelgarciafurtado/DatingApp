using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        //pega a variavel TokenKey que esta no arquivo de configuração chamado appsettings.json
        var token = config["Tokenkey"] ?? throw new Exception("Cannot get token key");
        //valida o token
        if (token.Length < 64)
            throw new Exception("Your token key needs to be >= 64 characters");
        //install System.IdentityModel.Tokens.Jwt And microsoft.IdentityModel.Tokens
        //transforma o token em um arrau de Bytes padrao UTF8
        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));

        //cria informações sobre o usuario
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        //cria as credenciais usando a chave com base no algortimo para gerar as credenciais
        var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(tokenKey);
    }
}
