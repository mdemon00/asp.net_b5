using DataImporter.Data;
using System.Collections.Generic;

namespace DataImporter.Importing.Entities
{
    public class Row : IEntity<int>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public IList<Cell> Cells { get; set; }

    }
}
