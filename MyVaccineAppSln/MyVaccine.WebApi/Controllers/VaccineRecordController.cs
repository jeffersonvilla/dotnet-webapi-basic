using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Dtos.VaccineRecord;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VaccineRecordController : ControllerBase
{
    private readonly IVaccineRecordService _vaccineRecordService;
    private readonly IValidator<VaccineRecordRequestDto> _validator;

    public VaccineRecordController(IVaccineRecordService vaccineRecordService, 
        IValidator<VaccineRecordRequestDto> validator)
    {
        _vaccineRecordService = vaccineRecordService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(VaccineRecordRequestDto vaccineRecordsDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineRecordsDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var vaccineRecords = await _vaccineRecordService.Add(vaccineRecordsDto);
        return Ok(vaccineRecords);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<VaccineRecordResponseDto> vaccineRecordCategories = await _vaccineRecordService.GetAll();
        return Ok(vaccineRecordCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            VaccineRecordResponseDto vaccineRecord = await _vaccineRecordService.GetById(id);
            return Ok(vaccineRecord);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VaccineRecordRequestDto vaccineRecordDto)
    {
        var validationResult = await _validator.ValidateAsync(vaccineRecordDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            VaccineRecordResponseDto updatedVaccineRecord = await _vaccineRecordService.Update(vaccineRecordDto, id);
            return Ok(updatedVaccineRecord);
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
            VaccineRecordResponseDto deletedVaccineRecord = await _vaccineRecordService.Delete(id);
            return Ok(deletedVaccineRecord);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}


