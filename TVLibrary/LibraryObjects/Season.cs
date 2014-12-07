using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVLibrary.LibraryObjects
{
    public class Season
    {
        public int SeasonNumber { get; set; }
        public int EpisodeCount { get; set; }
        public HashSet<Episode> Episodes { get; set; }
    }
}
