using ComplaintSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService) => _lookupService = lookupService;

    [HttpGet("governorates")]
    public async Task<IActionResult> GetGovernorates() => Ok(await _lookupService.GetGovernoratesAsync());

    [HttpGet("centers")]
    public async Task<IActionResult> GetCenters([FromQuery] int? governorateId) =>
        Ok(await _lookupService.GetCentersAsync(governorateId));

    [HttpGet("sectors")]
    public async Task<IActionResult> GetSectors() => Ok(await _lookupService.GetSectorsAsync());

    [HttpGet("services")]
    public async Task<IActionResult> GetServices([FromQuery] int? sectorId) =>
        Ok(await _lookupService.GetServicesAsync(sectorId));

    [HttpGet("government-entities")]
    public async Task<IActionResult> GetGovernmentEntities() => Ok(await _lookupService.GetGovernmentEntitiesAsync());
}
