using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Training
{
    public class DuplicateTitleException : Exception
    {
        public DuplicateTitleException(string message) : base(message) { }
    }
    class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message)
            :base(message) { }
    }
}
