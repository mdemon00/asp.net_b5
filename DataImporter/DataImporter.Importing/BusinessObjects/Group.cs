
using System;

namespace DataImporter.Importing.BusinessObjects
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ApplicationUserId { get; set; }
    }
}
