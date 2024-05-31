using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.Vaccine;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccineController : ControllerBase
{
    private readonly IVaccineService _vaccineService;
    private readonly IValidator<VaccineRequestDto> _validator;

    public VaccineController(IVaccineService vaccineService, IValidator<VaccineRequestDto> validator)
    {
        _vaccineService = vaccineService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineRequestDto vaccinesDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccinesDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var vaccines = await _vaccineService.Add(vaccinesDto);
        return Ok(vaccines);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<VaccineResponseDto> vaccineCategories = await _vaccineService.GetAll();
        return Ok(vaccineCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            VaccineResponseDto vaccine = await _vaccineService.GetById(id);
            return Ok(vaccine);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineRequestDto vaccineDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            VaccineResponseDto updatedVaccine = await _vaccineService.Update(vaccineDto, id);
            return Ok(updatedVaccine);
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
            VaccineResponseDto deletedVaccine = await _vaccineService.Delete(id);
            return Ok(deletedVaccine);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    [HttpPut("{vaccineId}/setCategory/{categoryId}")]
    public async Task<IActionResult> SetVaccineCategory(int vaccineId, int categoryId)
    {
        try
        {
            var updatedVaccine = await _vaccineService.SetVaccineCategory(vaccineId, categoryId);
            return Ok(updatedVaccine);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpDelete("{vaccineId}/categories/{categoryId}")]
    public async Task<IActionResult> RemoveVaccineCategory(int vaccineId, int categoryId)
    {
        try
        {
            var updatedVaccine = await _vaccineService.RemoveVaccineCategory(vaccineId, categoryId);
            return Ok(updatedVaccine);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
