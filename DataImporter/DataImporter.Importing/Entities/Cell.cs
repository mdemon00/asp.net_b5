using DataImporter.Data;

namespace DataImporter.Importing.Entities
{
    public class Cell : IEntity<int>
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public int RowId { get; set; }
        public Row Row { get; set; }
    }
}
