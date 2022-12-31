using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat.Journal
{
    public class JournalFileheader : JournalBase
    {
        public int part { get; set; }
        public string language { get; set; }
        public bool Odyssey { get; set; }
        public string gameversion { get; set; }
        public string build { get; set; }
    }
}
