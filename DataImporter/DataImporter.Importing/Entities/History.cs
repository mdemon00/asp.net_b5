using System;

namespace DataImporter.Importing.Entities
{
    public class History
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string status { get; set; }

        public string GroupName { get; set; }
        public Group Group { get; set; }
    }
}
