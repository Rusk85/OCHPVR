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
        public HashSet<Season> Seasons { get; set; }
    }
}
