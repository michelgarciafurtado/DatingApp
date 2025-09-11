using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<AppUser>> Register([FromBody] RegisterDto dto)
        {
            if (await VerifyEmail(dto.Email)) return BadRequest("Email taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Email = dto.Email,
                Displayname = dto.Displayname,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == dto.EMail);

            if (user == null) return Unauthorized("Invalid E-mail Address!");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (var i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != user.Password[i]) return Unauthorized("Invalid password");
            }
            var userdto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                DisplayNme = user.Displayname,
                Token = tokenService.CreateToken(user)
            };

            return userdto;
        }


        private async Task<bool> VerifyEmail(string email)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

    }
}
