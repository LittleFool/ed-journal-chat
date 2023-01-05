using ed_journal_chat.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class CMDR
    {
        public static string? Name { get; set; }
        public static JournalFSDTarget? FSDTarget { get; set; }
        public static string? LastReceivedText { get; set; }
        public static string? LastSentText { get; set; }
        public static bool Legacy { get; set; }
    }
}
