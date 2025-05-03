using Application.Service.Interface;
using Application.ViewModel;
using Core.Entity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;
[Route("api/v1/contacts")]
[ApiController]
public class ContactController : ControllerBase
{
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

            return Ok(new ResultViewModel<IList<Contact>>(contacts));
        }
        catch (SystemException)
        {
            // 01P01 é um código único qualquer que facilita identificar onde o erro foi gerado (Uma boa prática)
            return StatusCode(500, new ResultViewModel<IList<Contact>>("01P01 - Internal server error")); 
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetByIdAsync(int id)
    {
        try
        {
            var contact = await _contactService.GetByIdAsync(id);

            if (contact is null)
                return NoContent();

            return Ok(new ResultViewModel<Contact>(contact));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Contact>("01P02- Internal server error"));
        }
    }

    [HttpGet("ddd-code/{id:int}")]
    public async Task<ActionResult<List<Contact>>> GetAllByDddAsync(int id)
    {
        try
        {
            var contacts = await _contactService.GetAllByDddAsync(id);

            return Ok(new ResultViewModel<IList<Contact>>(contacts));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Contact>("01P04- Internal server error"));
        }
    }

    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] ContactViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ResultViewModel<Contact>(ModelState.GetErrors()));
            }

            var contact = new Contact
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                DddId = model.DddId,
            };

            await _contactService.CreateAsync(contact);

            return Created($"api/v1/contacts/{contact.Id}", new ResultViewModel<Contact>(contact));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Contact>("01P05 - Internal server error"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ContactViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Contact>(ModelState.GetErrors()));
            }

            var contact = await _contactService.GetByIdAsync(id);
            
            if (contact is null)
                return BadRequest(new ResultViewModel<Contact>("01P06 - Invalid contact id"));

            contact.Name = model.Name;
            contact.Phone = model.Phone;
            contact.Email = model.Email;
            contact.DddId = model.DddId;

            await _contactService.EditAsync(contact);
                        
            return Ok(new ResultViewModel<Contact>(contact));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Contact>("01P07 - Internal server error"));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var contact = await _contactService.GetByIdAsync(id);

            if (contact is null)
                return BadRequest(new ResultViewModel<Contact>("01P08 - Invalid contact id"));

            await _contactService.DeleteAsync(contact);

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Contact>("01P09 - Internal server error"));
        }
    }
}
