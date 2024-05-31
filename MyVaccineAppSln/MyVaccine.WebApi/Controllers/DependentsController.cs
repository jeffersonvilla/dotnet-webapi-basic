using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;
    private readonly IValidator<DependentRequestDto> _validator;

    public DependentsController(IDependentService dependentService, IValidator<DependentRequestDto> validator)
    {
        _dependentService = dependentService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dependents = await _dependentService.GetAll();
        return Ok(dependents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dependents = await _dependentService.GetById(id);
        return Ok(dependents);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DependentRequestDto dependentsDto)
    {
        var validationResult = await _validator.ValidateAsync(dependentsDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var dependents = await _dependentService.Add(dependentsDto);
        return Ok(dependents);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DependentRequestDto dependentDto)
    {
        var validationResult = await _validator.ValidateAsync(dependentDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            DependentResponseDto updatedDependent = await _dependentService.Update(dependentDto, id);
            return Ok(updatedDependent);
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
            DependentResponseDto deletedDependent = await _dependentService.Delete(id);
            return Ok(deletedDependent);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
