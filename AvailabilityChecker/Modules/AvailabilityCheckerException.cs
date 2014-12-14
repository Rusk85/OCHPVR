using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailabilityChecker.Modules
{
    public class AvailabilityCheckerException : Exception
    {
        public AvailabilityCheckerException(string Message) 
            : base (Message) { }

        public AvailabilityCheckerException(string Message, Exception InnerException)
            : base(Message, InnerException) { }
    }
}
