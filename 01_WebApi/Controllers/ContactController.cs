using Application.Service.Interface;
using Core.Entity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("persistence-api/v1/contacts")]
[ApiController]
public class ContactController : ControllerBase
{
    // Controller de persistência
    private readonly IContactService _contactService;    

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;        
    }

    [HttpGet]
    public async Task<ActionResult<List<Contact>>> GetAllAsync()
    {
        try
        {
            var contacts = await _contactService.GetAllAsync();

            return Ok(contacts);
        }
        catch (SystemException)
        {
            return StatusCode(500, "01P01 - Internal server error"); 
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetByIdAsync(int id)
    {
        try
        {
            var contact = await _contactService.GetByIdAsync(id);

            return Ok(contact);
        }
        catch
        {
            return StatusCode(500, "01P02- Internal server error");
        }
    }

    [HttpGet("ddd-code/{id:int}")]
    public async Task<ActionResult<List<Contact>>> GetAllByDddAsync(int id)
    {
        try
        {
            var contacts = await _contactService.GetAllByDddAsync(id);

            return Ok(contacts);
        }
        catch (Exception)
        {
            return StatusCode(500, "01P03- Internal server error");
        }
    }

    [HttpGet("persistence-error-test/{fail:bool}")]
    public async Task<ActionResult<string>> PersistanceApiErrorTest(bool fail)
    {
        if (fail)
        {
            return StatusCode(500, "01P04- Internal server error");
        }

        return Ok("Persistance Api is working again");
    }

    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] Contact contact)
    {
        try
        {
           await _contactService.CreateAsync(contact);

           return Created($"persistence-api/v1/contacts/{contact.Id}", contact);
        }
        catch (Exception)
        {
            return StatusCode(500, "01P05 - Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Contact contact)
    {
        try
        {            
            await _contactService.EditAsync(contact);
                        
            return Ok(contact);
        }
        catch (Exception)
        {
            return StatusCode(500, "01P06 - Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var contact = await _contactService.GetByIdAsync(id);
                
            if (contact is null)
                return BadRequest("01P07 - Invalid contact id");

            await _contactService.DeleteAsync(contact);

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "01P08 - Internal server error");
        }
    }
}
