using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyVaccine.WebApi.Dtos;
using MyVaccine.WebApi.Literals;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(UserManager<IdentityUser> userManager, IUserService userService)
    {
      _userService= userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequetDto model)
    {       
        var response = await _userService.AddUserAsync(model);
        if ( response.IsSuccess) {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var response = await _userService.Login(model);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return Unauthorized(response);
        }


    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var response = await _userService.RefreshToken(claimsIdentity.Name);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return Unauthorized(response);
        }


    }
}


