using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Searching
{
    public interface IIndexer
    {
        string Indexer { get; }
    }

    public class TehParadoxIndexer : IIndexer
    {
        public string Indexer
        {
            get { return "tehparadox.com"; }
        }
    }


}
