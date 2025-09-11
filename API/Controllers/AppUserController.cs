using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AppUserController(AppDbContext context) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> Get()
        {
            return await context.Users.ToListAsync();
        }
        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<AppUser>> GetMember(string Id)
        {
            var member = await context.Users.FindAsync(Id);
            if (member == null)
                return NotFound() ;

            return member;    
        }
    }
}
