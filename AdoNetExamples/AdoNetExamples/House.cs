using System.Collections.Generic;

namespace AdoNetExamples
{
    public class House : IData
    {
        public int Id { get; set; }
        public IList<Room> Room { get; set; }
    }
}
