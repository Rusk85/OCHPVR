using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Searching
{
    public class SearchResult
    {
        public Url Link { get; set; }
        public string Snippet { get; set; }
    }
}
