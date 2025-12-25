using DotnetPgDemo.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetPgDemo.Api.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("GetPersonById", new { id = person.id}, person);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var people = await _context.People.ToListAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                
            }
        }

        [HttpGet("{id}", Name = "GetPersonById")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {
                var person = await _context.People.FindAsync(id);

                if (person is null)
                {
                    return NotFound($"Person with id : {id} does not exist");
                }
                return Ok(person);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            try
            {
                if (id != person.id)
                {
                    return BadRequest($"Id in the query does not match id in the body");
                }

                var personExists = _context.People.Any(p=>p.id==id);

                if (!personExists)
                {
                    return NotFound($"Person with id: {id} does not exist.");
                }

                _context.People.Update(person);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {

                var person = await _context.People.FindAsync(id);

                if (person is null)
                {
                    return NotFound($"Person with id: {id} does not exist.");
                }

                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
