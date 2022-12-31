using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat.Journal
{
    internal class JournalSendText : JournalBase
    {
        public string To { get; set; }
        public string Message { get; set; }
        public bool Sent { get; set; }
    }
}
