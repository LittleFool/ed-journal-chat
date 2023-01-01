using ed_journal_chat.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ed_journal_chat
{
    internal class DisplayOutput
    {
        public static void Fileheader(JournalFileheader journalFileheader)
        {
            string mode;
            if (journalFileheader.Odyssey)
                mode = "Odyssey";
            else
                mode = "Horizons";

            Console.WriteLine("This is " + mode + " version " + journalFileheader.gameversion + " build " + journalFileheader.build);
        }

        public static void SendText(JournalSendText journalObject)
        {
            string to = journalObject.To;

            switch (journalObject.To)
            {
                case "starsystem": to = "system"; break;
                case "wing": to = "team"; break;
            }

            if (journalObject.Sent)
                Console.WriteLine(journalObject.timestamp.TimeOfDay + " *" + to + "* Me: " + journalObject.Message);
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

            Console.WriteLine(journalObject.timestamp.TimeOfDay + " *" + channel + "* " + journalObject.From + ": " + journalObject.Message);
        }

        public static void Friends(JournalFriends journalFriends)
        {
            switch(journalFriends.Status)
            {
                case "Requested":
                    Console.WriteLine(journalFriends.timestamp.TimeOfDay + " friend request from " + journalFriends.Name); break;

                case "Added":
                    Console.WriteLine(journalFriends.timestamp.TimeOfDay + " now friends with " + journalFriends.Name); break;
            }
        }

        public static void WingInvite(JournalWingInvite journalWingInvite)
        {
            Console.WriteLine(journalWingInvite.timestamp.TimeOfDay + " wing invite from " + journalWingInvite.Name);
        }

        public static void Shutdown(JournalShutdown journalShutdown)
        {
            Console.WriteLine(journalShutdown.timestamp.TimeOfDay + " Game Closed");
        }
    }
}
