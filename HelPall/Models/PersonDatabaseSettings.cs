using System;
namespace Helpall.Models
{
    public class PersonDatabaseSettings : IPersonDatabaseSettings
    {
        public string PersonsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPersonDatabaseSettings
    {
        string PersonsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}