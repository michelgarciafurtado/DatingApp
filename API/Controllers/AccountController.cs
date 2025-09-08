using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController(AppDbContext context) : BaseApiController
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

        private async Task<bool> VerifyEmail(string email)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

    }
}
