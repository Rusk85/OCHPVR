using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvailabilityChecker.Modules
{
    public abstract class Checker : IChecker
    {

        protected IList<Url> SupportedHosters;

        public Checker(IList<Url> SupportedHosters)
        {
            this.SupportedHosters = SupportedHosters;
        }

        public abstract bool IsAvailable(Url Link);

        public abstract bool IsAvailable(IList<Url> Links);

        public bool IsSupported(Url Hoster)
        {
            if (SupportedHosters == null)
            {
                throw new AvailabilityCheckerException(
                    "No supported hosters for this checker provided");
            }
            return SupportedHosters.Select(sh => sh.Path)
                .Any(p => p.ToLower() == Hoster.Path.ToLower());
        }
    }

    public abstract class HttpChecker : Checker
    {
        protected Url BaseUrl;

        public HttpChecker(IList<Url> SupportedHosters, Url BaseUrl) 
            : base(SupportedHosters) { this.BaseUrl = BaseUrl; }

        public override abstract bool IsAvailable(Url Link);
        public override abstract bool IsAvailable(IList<Url> Links);
    }



}
