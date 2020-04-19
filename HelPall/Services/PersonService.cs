using Helpall.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpall.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _persons;

        public PersonService(IPersonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _persons = database.GetCollection<Person>(settings.PersonsCollectionName);
        }

        public List<Person> Get() =>
            _persons.Find(person => true).ToList();

        public Person Get(Guid id) =>
            _persons.Find<Person>(person => person.Id == id).FirstOrDefault();

        public Person Create(Person person)
        {
            _persons.InsertOne(person);
            return person;
        }

        public void Update(Guid id, Person personIn) =>
            _persons.ReplaceOne(person => person.Id == id, personIn);

        public void Remove(Person personIn) =>
            _persons.DeleteOne(person => person.Id == personIn.Id);

        public void Remove(Guid id) =>
            _persons.DeleteOne(person => person.Id == id);
    }
}
