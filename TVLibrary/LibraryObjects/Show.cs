using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVLibrary.LibraryObjects
{



    public class Show
    {
        public string Name { get; set; }
        public int LibraryManagerSpecificId { get; set; }
        public AirStatus Status { get; set; }
        public HashSet<Season> Seasons { get; set; }

        public enum AirStatus
        {
            Continuing,
            Ended
        }




    }
}
