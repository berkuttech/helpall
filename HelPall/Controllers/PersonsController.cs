﻿
using HelPall.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HelPall.Services;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;


namespace HelPall.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly PersonService _personService;
        private readonly ILogger<PersonsController> _logger;
        private readonly IStringLocalizer<PersonsController> _localizer;
        private readonly IStringLocalizer<SharedResource> _localizerShared;

        //private readonly ResourceManager _resourceManager;

        public PersonsController(PersonService personService, ILogger<PersonsController> logger, IStringLocalizer<PersonsController> localizer, IStringLocalizer<SharedResource> localizerShared)
        {
            _personService = personService;
            _logger = logger;
            _localizer = localizer;
            _localizerShared = localizerShared;

        }

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _personService.Get();

        [HttpGet("{id}", Name = "GetPerson")]
        public ActionResult<Person> Get(Guid id)
        {
            _logger.LogInformation("Person with Id" + id.ToString() + "is retrieved at ", DateTime.UtcNow);

            var person = _personService.Get(id);

            if (person == null)
            {
                _logger.LogError(nameof(Get) + _localizer["PersonNotFound"] + id);
                return new NotFoundObjectResult(String.Format(_localizer["PersonNotFound"].Value, id));

            }

            return Ok(person);
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