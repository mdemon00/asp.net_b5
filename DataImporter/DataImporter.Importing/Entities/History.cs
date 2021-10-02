using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;

namespace DataImporter.Importing.Entities
{
    public class History : IEntity<int>
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string ProcessType { get; set; }
        public string Email { get; set; }
        public int EmailSent { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
