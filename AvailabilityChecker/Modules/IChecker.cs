using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailabilityChecker.Modules
{
    public interface IChecker
    {
        bool IsSupported(Url Hoster);
        bool IsAvailable(Url Link);
        bool IsAvailable(IList<Url> Links);
    }
}
