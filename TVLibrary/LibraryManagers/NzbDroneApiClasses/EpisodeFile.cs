﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVLibrary.LibraryManagers.NzbDroneApiClasses
{
    public class Quality2
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Revision
    {
        public int version { get; set; }
        public int real { get; set; }
    }

    public class Quality
    {
        public Quality2 quality { get; set; }
        public Revision revision { get; set; }
    }

    public class NzbDroneEpisodeFile
    {
        public int seriesId { get; set; }
        public int seasonNumber { get; set; }
        public string relativePath { get; set; }
        public string path { get; set; }
        public Int64 size { get; set; }
        public string dateAdded { get; set; }
        public Quality quality { get; set; }
        public bool qualityCutoffNotMet { get; set; }
        public int id { get; set; }
        public string sceneName { get; set; }
    }
}
