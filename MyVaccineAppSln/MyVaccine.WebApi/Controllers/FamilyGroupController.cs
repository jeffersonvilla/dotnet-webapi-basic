using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyUser.WebApi.Services.Contracts;
using MyVaccine.WebApi.Dtos.FamilyGroup;

namespace MyFamilyGroup.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FamilyGroupController : ControllerBase
{
    private readonly IFamilyGroupService _familyGroupService;
    private readonly IValidator<FamilyGroupRequestDto> _validator;

    public FamilyGroupController(IFamilyGroupService familyGroupService, IValidator<FamilyGroupRequestDto> validator)
    {
        _familyGroupService = familyGroupService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(FamilyGroupRequestDto familyGroupsDto)
    {
        var validationResult = await _validator.ValidateAsync(familyGroupsDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var familyGroups = await _familyGroupService.Add(familyGroupsDto);
        return Ok(familyGroups);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<FamilyGroupResponseDto> familyGroupCategories = await _familyGroupService.GetAll();
        return Ok(familyGroupCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            FamilyGroupResponseDto familyGroup = await _familyGroupService.GetById(id);
            return Ok(familyGroup);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FamilyGroupRequestDto familyGroupDto)
    {
        var validationResult = await _validator.ValidateAsync(familyGroupDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            FamilyGroupResponseDto updatedFamilyGroup = await _familyGroupService.Update(familyGroupDto, id);
            return Ok(updatedFamilyGroup);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            FamilyGroupResponseDto deletedFamilyGroup = await _familyGroupService.Delete(id);
            return Ok(deletedFamilyGroup);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    [HttpPut("{familyGroupId}/set/{userId}")]
    public async Task<IActionResult> SetFamilyGroup(int familyGroupId, int userId)
    {
        try
        {
            var updatedFamilyGroup = await _familyGroupService.SetUser(familyGroupId, userId);
            return Ok(updatedFamilyGroup);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpDelete("{familyGroupId}/user/{userId}")]
    public async Task<IActionResult> RemoveFamilyGroup(int familyGroupId, int userId)
    {
        try
        {
            var updatedFamilyGroup = await _familyGroupService.RemoveUser(familyGroupId, userId);
            return Ok(updatedFamilyGroup);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
