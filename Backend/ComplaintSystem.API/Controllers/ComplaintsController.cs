using ComplaintSystem.Application.Common;
using ComplaintSystem.Application.DTOs.Complaints;
using ComplaintSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintService _complaintService;

    public ComplaintsController(IComplaintService complaintService) => _complaintService = complaintService;

    // بيتنادى من الفرونت (Angular) وهو الموظف لسه بيكتب في خانة الوصف - قبل الإرسال النهائي
    // عشان يظهر تحذير التكرار فورًا
    [HttpPost("check-duplicates")]
    public async Task<IActionResult> CheckDuplicates([FromBody] CheckDuplicatesRequest request)
    {
        var result = await _complaintService.CheckDuplicatesAsync(request.Description);
        return Ok(ApiResponse<DuplicateCheckResultDto>.Ok(result));
    }

    // إنشاء الشكوى فعليًا - لو فيه تكرار مؤكد ومحددش ConfirmDespiteDuplicateWarning هيرجع 409
    // مع تفاصيل الشكاوى المشابهة عشان الفرونت يعرض التحذير
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateComplaintRequest request, [FromForm] List<IFormFile>? files)
    {
        var fileTuples = new List<(Stream content, string fileName, string contentType)>();
        if (files != null)
            foreach (var f in files)
                fileTuples.Add((f.OpenReadStream(), f.FileName, f.ContentType));

        var result = await _complaintService.CreateAsync(request, fileTuples);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ComplaintDetailsDto>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(ApiResponse<ComplaintDetailsDto>.Ok(await _complaintService.GetByIdAsync(id)));

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ComplaintFilterRequest filter) =>
        Ok(await _complaintService.GetListAsync(filter));

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateComplaintStatusRequest request)
    {
        await _complaintService.UpdateStatusAsync(id, request);
        return NoContent();
    }

    [HttpPut("{id:int}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(int id, AssignComplaintRequest request)
    {
        await _complaintService.AssignAsync(id, request);
        return NoContent();
    }
}

public class CheckDuplicatesRequest
{
    public string Description { get; set; } = default!;
}
