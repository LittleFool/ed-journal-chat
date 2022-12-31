using ed_journal_chat.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class DisplayOutput
    {
        public static void SendText(JournalSendText journalObject)
        {
            if (journalObject.Sent)
                Console.WriteLine(journalObject.timestamp + " *" + journalObject.To + "* Me: " + journalObject.Message);
        }

        public static void ReceiveText(JournalReceiveText journalObject)
        {
            // dont display NPC messages e.g. Cruise Ship, Entering System notifications and so on
            if (journalObject.Channel.Equals("npc"))
                return;

            string channel = journalObject.Channel;

            switch(journalObject.Channel)
            {
                case "player": channel = "direct"; break;
                case "starsystem": channel = "system"; break;
                case "wing": channel = "team"; break;
            }

            Console.WriteLine(journalObject.timestamp + " *" + channel + "* " + journalObject.From + ": " + journalObject.Message);
        }
    }
}
