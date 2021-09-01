using System;

namespace StockData.Scraping.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message)
            : base(message) { }
    }
}
