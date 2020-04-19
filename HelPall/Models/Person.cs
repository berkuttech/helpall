using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helpall.Models
{
    public class Person
    {

        public Guid Id { get; set; }

        [BsonElement("Name")]
        public string FullName { get; set; }

        public int Age { get; set; }

        public int Type { get; set; }

    }
}
