using Glimpse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Helpers
{
    public class LocationChangedHandlerArgs
    {
        public LocationChangedHandlerArgs(Location location)
        {
            Location = location;
         
        }

        public Location Location { get; private set; }
    }
}
