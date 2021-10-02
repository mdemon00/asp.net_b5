
using System;

namespace DataImporter.Importing.BusinessObjects
{
    public class History
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int GroupId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string ProcessType { get; set; }
        public string Email { get; set; }
        public bool EmailSent { get; set; }

        public Guid ApplicationUserId { get; set; }
    }
}
