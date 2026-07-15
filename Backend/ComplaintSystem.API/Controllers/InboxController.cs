using ComplaintSystem.Application.DTOs.Complaints;
using ComplaintSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintSystem.API.Controllers;

// صندوق الوارد - الشكاوى المعينة للمستخدم الحالي أو فريقه
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InboxController : ControllerBase
{
    private readonly IComplaintService _complaintService;

    public InboxController(IComplaintService complaintService) => _complaintService = complaintService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ComplaintFilterRequest filter) =>
        Ok(await _complaintService.GetInboxAsync(filter));
}
