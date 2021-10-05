using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;

namespace DataImporter.Importing.Entities
{
    public class Group : IEntity<int>
    {
        public int Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }
        public IList<Row> Rows { get; set; }
        public IList<History> Histories { get; set; }
    }
}
