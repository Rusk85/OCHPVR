using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;

namespace TVLibrary.LibraryManagers
{
    public interface ILibraryManager
    {
        List<Episode> GetMissingEpisodes();
        List<Episode> GetMissingEpisodes(Show Show);
        List<Episode> GetMissingEpisodes(Show Show, Season Season);
        List<Episode> GetMissingEpisodes(DateTime Date);
    }
}
