﻿using DataImporter.Data;
using System.Collections.Generic;

namespace DataImporter.Importing.Entities
{
    public class Group : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }
    }
}
