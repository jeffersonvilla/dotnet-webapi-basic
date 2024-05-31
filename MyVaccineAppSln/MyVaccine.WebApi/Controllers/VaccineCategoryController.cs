using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.VaccineCategory;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Services.Contracts;
using MyVaccine.WebApi.Services.Implementations;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccineCategoryController : ControllerBase
{
    private readonly IVaccineCategoryService _vaccineCategoryService;
    private readonly IValidator<VaccineCategoryRequestDto> _validator;

    public VaccineCategoryController(IVaccineCategoryService vaccineCategoryService, IValidator<VaccineCategoryRequestDto> validator)
    {
        _vaccineCategoryService = vaccineCategoryService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineCategoryRequestDto vaccineCategorysDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineCategorysDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var vaccineCategorys = await _vaccineCategoryService.Add(vaccineCategorysDto);
        return Ok(vaccineCategorys);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<VaccineCategoryResponseDto> vaccineCategories = await _vaccineCategoryService.GetAll();
        return Ok(vaccineCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            VaccineCategoryResponseDto vaccineCategory = await _vaccineCategoryService.GetById(id);
            return Ok(vaccineCategory);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineCategoryRequestDto vaccineCategoryDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineCategoryDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            VaccineCategoryResponseDto updatedVaccineCategory = await _vaccineCategoryService.Update(vaccineCategoryDto, id);
            return Ok(updatedVaccineCategory);
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
            VaccineCategoryResponseDto deletedVaccineCategory = await _vaccineCategoryService.Delete(id);
            return Ok(deletedVaccineCategory);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{categoryId}/setVaccine/{vaccineId}")]
    public async Task<IActionResult> SetVaccineCategory(int categoryId,int vaccineId)
    {
        try
        {
            var updatedVaccineCategory = 
                await _vaccineCategoryService.SetVaccine(categoryId, vaccineId);

            return Ok(updatedVaccineCategory);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpDelete("{categoryId}/vaccine/{vaccineId}")]
    public async Task<IActionResult> RemoveVaccineCategory(int categoryId, int vaccineId)
    {
        try
        {
            var updatedVaccineCategory = 
                await _vaccineCategoryService.RemoveVaccine(categoryId, vaccineId);

            return Ok(updatedVaccineCategory);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

}
