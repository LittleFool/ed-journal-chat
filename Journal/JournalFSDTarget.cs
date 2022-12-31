using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat.Journal
{
    public class JournalFSDTarget : JournalBase
    {
        public string Name { get; set; }
        public long SystemAddress { get; set; }
        public string StarClass { get; set; }
        public int RemainingJumpsInRoute { get; set; }
    }
}
