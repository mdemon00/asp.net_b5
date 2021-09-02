using StockData.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockData.Scraping.Entites
{
    public class Company : IEntity<int>
    {
        public int Id { get; set; }
        public string TradeCode { get; set; }
    }
}
