using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.Allergy;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Services.Contracts;
using MyVaccine.WebApi.Services.Implementations;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AllergyController : ControllerBase
{
    private readonly IAllergyService _allergyService;
    private readonly IValidator<AllergyRequestDto> _validator;

    public AllergyController(IAllergyService allergyService, IValidator<AllergyRequestDto> validator)
    {
        _allergyService = allergyService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AllergyRequestDto allergysDto)
    {

        var validationResult = await _validator.ValidateAsync(allergysDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var allergys = await _allergyService.Add(allergysDto);
        return Ok(allergys);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<AllergyResponseDto> allergyCategories = await _allergyService.GetAll();
        return Ok(allergyCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            AllergyResponseDto allergy = await _allergyService.GetById(id);
            return Ok(allergy);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AllergyRequestDto allergyDto)
    {
        var validationResult = await _validator.ValidateAsync(allergyDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            AllergyResponseDto updatedAllergy = await _allergyService.Update(allergyDto, id);
            return Ok(updatedAllergy);
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
            AllergyResponseDto deletedAllergy = await _allergyService.Delete(id);
            return Ok(deletedAllergy);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
}

