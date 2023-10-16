using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.Services;

namespace MyVaccine.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GuidController : ControllerBase
{
    private readonly IGuidGeneratorScope _scopeService;
    private readonly IGuidGeneratorTrasient _transientService;
    private readonly IGuidGeneratorSingleton _singletonService;
    private readonly IGuidGeneratorDeep _guidGeneratorDeep;

    public GuidController(IGuidGeneratorScope scopeService, IGuidGeneratorTrasient transientService, IGuidGeneratorSingleton singletonService, IGuidGeneratorDeep guidGeneratorDeep)
    {
        _scopeService = scopeService;
        _transientService = transientService;
        _singletonService = singletonService;
        _guidGeneratorDeep = guidGeneratorDeep;
    }

    [HttpGet("scope")]
    public IActionResult GetGuidScope()
    {
        var guid = _scopeService.GetGuid();
        var response = _guidGeneratorDeep.GetGuidDeep();
        response.ControllerGuid = guid;
        return Ok(response);
    }

    [HttpGet("transient")]
    public IActionResult GetGuidTransient()
    {
        var guid = _transientService.GetGuid();
        var response = _guidGeneratorDeep.GetGuidDeep();
        response.ControllerGuid = guid;
        return Ok(response);
    }

    [HttpGet("singleton")]
    public IActionResult GetGuidSingleton()
    {
        var guid = _singletonService.GetGuid();
        var response = _guidGeneratorDeep.GetGuidDeep();
        response.ControllerGuid= guid;
        return Ok(response);
    }
}
