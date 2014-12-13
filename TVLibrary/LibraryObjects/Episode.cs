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
        public string Title { get; set; }
        public DateTime AirDate { get; set; }
        public Season Season { get; set; }
        public Show Show { get; set; }
        public string EpisodeIndex
        {
            get
            {
                return String.Format("S{0}E{1}", 
                    Season.SeasonNumber.ToString().PadLeft(2,'0'),
                    EpisodeNumber.ToString().PadLeft(2, '0'));
            }
        }
    }
}
