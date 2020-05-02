using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaBot
{
    public class UserProfile
    {
        public string Name { get; set; }
        public string Team { get; set; }

        public List<string> InternalSystems { get; set; } = new List<string>();
    }
}
