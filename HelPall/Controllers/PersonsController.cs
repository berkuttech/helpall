
using Helpall.Models;
using Helpall;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Helpall.Services;
using System;

namespace Helpall.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonsController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _personService.Get();

        [HttpGet("{id}", Name = "GetPerson")]
        public ActionResult<Person> Get(Guid id)
        {
            var book = _personService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Person> Create(Person book)
        {
            _personService.Create(book);

            return CreatedAtRoute("GetPerson", new { id = book.Id.ToString() }, book);
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