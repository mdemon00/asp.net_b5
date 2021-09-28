
using System;

namespace DataImporter.Importing.BusinessObjects
{
    public class History
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string ProcessType { get; set; }

        public Guid ApplicationUserId { get; set; }
    }
}
