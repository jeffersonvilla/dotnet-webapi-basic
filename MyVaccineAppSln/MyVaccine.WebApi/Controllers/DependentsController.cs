using System.Security.AccessControl;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Models;
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

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dependents = await _dependentService.GetById(id);
        return Ok(dependents);
    }

    [HttpGet("get-dependents-by-userid/{userId}")]
    public async Task<IActionResult> GetDependensByUserId(int userId)
    {
        var dependents = await _dependentService.GetDependentsByUserId(userId);
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

    //    var dependent = _mapper.Map<Dependent>(dependentsDto);
    //    await _dependentRepository.Add(dependent);

    //    return CreatedAtAction(nameof(GetById), new { id = dependent.Id }, dependent);
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DependentRequestDto dependentsDto)
    {

        var dependent =  await _dependentService.Update(dependentsDto,id);
        if (dependent == null)
        {
            return NotFound();
        }       

        return Ok(dependent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete( int id)
    {
        var dependent = await _dependentService.Delete(id);
        if (dependent == null)
        {
            return NotFound();
        }

        return Ok(dependent);
    }
}
