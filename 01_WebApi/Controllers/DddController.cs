using Application.Service.Interface;
using Core.Entity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("persistence-api/v1/ddd")]
[ApiController]
public class DddController : ControllerBase
{
    private readonly IDirectDistanceDialingService _dddService;

    public DddController(IDirectDistanceDialingService dddService)
    {
        _dddService = dddService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetByIdAsync(int id)
    {
        try
        {
            var contact = await _dddService.GetByIdAsync(id);

            return Ok(contact);
        }
        catch
        {
            return StatusCode(500, "01P02- Internal server error");
        }
    }
}
