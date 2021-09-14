using DataImporter.Data;

namespace DataImporter.Importing.Entities
{
    public class Column: IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
