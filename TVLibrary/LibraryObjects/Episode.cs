using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVLibrary.LibraryObjects
{
    public class Episode
    {
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public Season Season { get; set; }
        public string EpisodeIndex
        {
            get
            {
                return String.Format("E{0}S{1}", 
                    EpisodeNumber, Season.SeasonNumber);
            }
        }
    }
}
