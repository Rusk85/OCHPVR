using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailabilityChecker.Modules
{

    public class CheckerResult
    {
        public Url Target { get; set; }
        public bool IsAvailable { get; set; }
    }

    public interface IChecker
    {
        bool IsSupported(Url Hoster);
        IList<Url> GetSupportedLinks(IList<Url> Links);
        bool IsAvailable(Url Link);
        bool IsAvailable(IList<Url> Links);
        IList<CheckerResult> GetAvailability(IList<Url> Links);
    }
}
