using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaBot
{
    public class ConversationData
    {
        public string Timestamp { get; set; }

        public string ChannelId { get; set; }

        public bool PropmtedUserForName { get; set; } = false;
    }
}
