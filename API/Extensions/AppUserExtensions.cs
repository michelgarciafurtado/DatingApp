using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Email = user.Email,
            Id = user.Id,
            DisplayNme = user.Displayname,
            Token = tokenService.CreateToken(user)
        };
    }

}
