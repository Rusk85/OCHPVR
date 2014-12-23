using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class OCHPVRException : Exception
    {
        public OCHPVRException(string Message) 
            : base (Message) { }

        public OCHPVRException(string Message, Exception InnerException)
            : base(Message, InnerException) { }
    }

    public class OCHPVRInitializationEception : OCHPVRException
    {
        public OCHPVRInitializationEception(string Message) 
            : base ("An error during initialization of the OCHPVR Core occured: " + Message) { }

        public OCHPVRInitializationEception(string Message, Exception InnerException)
            : base("An error during initialization of the OCHPVR Core occured: " + Message, InnerException) { }
    }
}
