using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Services.Contracts;
using MyVaccine.WebApi.Services.Implementations;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("{userId}/family-group/{familyGroupId}")]
    public async Task<IActionResult> SetFamilyGroup(int userId, int familyGroupId)
    {
        try
        {
            var result = await _userService.SetFamilyGroup(userId, familyGroupId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{userId}/family-group/{familyGroupId}")]
    public async Task<IActionResult> RemoveFamilyGroup(int userId, int familyGroupId)
    {
        try
        {
            var result = await _userService.RemoveFamilyGroup(userId, familyGroupId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

