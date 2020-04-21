
using Helpall.Models;
using Helpall;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Helpall.Services;
using System;
using Microsoft.Extensions.Logging;


namespace Helpall.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonService _personService;
        ILogger<PersonsController> _logger;

        public PersonsController(PersonService personService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _logger = logger;

        }

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _personService.Get();

        [HttpGet("{id}", Name = "GetPerson")]
        public ActionResult<Person> Get(Guid id)
        {
            _logger.LogInformation("Person with Id" + id.ToString() + "is retrieved at ", DateTime.UtcNow);

            var book = _personService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Person> Create(Person person)
        {

            try
            {
                _logger.LogInformation("Person with Id" + person.Id.ToString() + "is created at ", DateTime.UtcNow);

                _personService.Create(person);

                return CreatedAtRoute("GetPerson", new { id = person.Id.ToString() }, person);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unknown error occurred on the Create Person action of the Persons");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Person bookIn)
        {
            var book = _personService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _personService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var book = _personService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _personService.Remove(book.Id);

            return NoContent();
        }
    }
}